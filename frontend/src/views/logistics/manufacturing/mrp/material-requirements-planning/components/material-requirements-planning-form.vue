<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/mrp/material-requirements-planning/components -->
<!-- 文件名称：material-requirements-planning-form.vue -->
<!-- 功能描述：物料需求计划 MRP 头表维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form material-requirements-planning-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="material-requirements-planning-form-tabs"
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
                  :disabled="!!formData?.materialRequirementsPlanningId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialRequirementsPlanningCode')"
                name="materialRequirementsPlanningCode"
              >
                <a-input
                  v-model:value="formState.materialRequirementsPlanningCode"
                  :placeholder="pi.ph('materialRequirementsPlanningCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.materialRequirementsPlanningId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('masterProductionScheduleId')"
                name="masterProductionScheduleId"
              >
                <a-input
                  v-model:value="formState.masterProductionScheduleId"
                  :placeholder="pi.ph('masterProductionScheduleId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('mpsCode')"
                name="mpsCode"
              >
                <a-input
                  v-model:value="formState.mpsCode"
                  :placeholder="pi.ph('mpsCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.materialRequirementsPlanningId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('masterDemandScheduleId')"
                name="masterDemandScheduleId"
              >
                <a-input
                  v-model:value="formState.masterDemandScheduleId"
                  :placeholder="pi.ph('masterDemandScheduleId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('mdsCode')"
                name="mdsCode"
              >
                <a-input
                  v-model:value="formState.mdsCode"
                  :placeholder="pi.ph('mdsCode')"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.materialRequirementsPlanningId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planDate')"
                name="planDate"
              >
                <a-date-picker
                  v-model:value="formState.planDate"
                  :placeholder="pi.ph('planDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planPeriodStart')"
                name="planPeriodStart"
              >
                <a-date-picker
                  v-model:value="formState.planPeriodStart"
                  :placeholder="pi.ph('planPeriodStart')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planPeriodEnd')"
                name="planPeriodEnd"
              >
                <a-date-picker
                  v-model:value="formState.planPeriodEnd"
                  :placeholder="pi.ph('planPeriodEnd')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('plannerId')"
                name="plannerId"
              >
                <TaktSelect
                  v-model:value="formState.plannerId"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('plannerId')"
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
                :label="pi.label('planBy')"
                name="planBy"
              >
                <TaktSelect
                  v-model:value="formState.planBy"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('planBy')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('runStatus')"
                name="runStatus"
              >
                <a-input-number
                  v-model:value="formState.runStatus"
                  :placeholder="pi.ph('runStatus')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('productionPlanId')"
                name="productionPlanId"
              >
                <a-input
                  v-model:value="formState.productionPlanId"
                  :placeholder="pi.ph('productionPlanId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('productionPlanCode')"
                name="productionPlanCode"
              >
                <a-input
                  v-model:value="formState.productionPlanCode"
                  :placeholder="pi.ph('productionPlanCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.materialRequirementsPlanningId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('purchasePlanId')"
                name="purchasePlanId"
              >
                <a-input
                  v-model:value="formState.purchasePlanId"
                  :placeholder="pi.ph('purchasePlanId')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('purchasePlanCode')"
                name="purchasePlanCode"
              >
                <a-input
                  v-model:value="formState.purchasePlanCode"
                  :placeholder="pi.ph('purchasePlanCode')"
                  show-count
                  :maxlength="10"
                  allow-clear
                  :disabled="!!formData?.materialRequirementsPlanningId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('planDescription')"
                name="planDescription"
              >
                <a-textarea
                  v-model:value="formState.planDescription"
                  :placeholder="pi.ph('planDescription')"
                  :rows="2"
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
      ref="materialRequirementsPlanningItemTableRef"
      v-model="childMaterialRequirementsPlanningItemRows"
      :columns="materialRequirementsPlanningItemFormColumns"
      :title="materialRequirementsPlanningItemPi.self()"
      :add-button-entity="materialRequirementsPlanningItemPi.self()"
      id-field="materialRequirementsPlanningItemId"
      :default-row="createDefaultMaterialRequirementsPlanningItemRow"
      :disabled="loading"
      :enable-vertical-scroll="false"
      section-border
      class="w-full min-w-0"
    >
      <template #cell-materialCode="{ record }">
        <TaktSelect
          v-model:value="record.materialCode"
          api-url="TaktMaterials/options"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialRequirementsPlanningItemPi.queryPh('materialCode', 'select')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-planUnit="{ record }">
        <TaktSelect
          v-model:value="record.planUnit"
          dict-type="logistics_unit_of_measure_code"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialRequirementsPlanningItemPi.ph('planUnit')"
          :disabled="loading"
          allow-clear
        />
      </template>
      <template #cell-procurementType="{ record }">
        <TaktSelect
          v-model:value="record.procurementType"
          dict-type="logistics_procurement_type"
          class="w-full"
          :get-popup-container="getSelectPopupContainer"
          :placeholder="materialRequirementsPlanningItemPi.ph('procurementType')"
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
          :placeholder="materialRequirementsPlanningItemPi.ph('isObsolete')"
          :disabled="loading"
          allow-clear
        />
      </template>
    </TaktEditableTable>
  </a-form>
</template>

<script setup lang="ts">
/**
 * 物料需求计划 MRP 头表维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/mrp/material-requirements-planning/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useMaterialRequirementsPlanningI18n } from '../composables/use-material-requirements-planning-i18n'

/** 实体字段 i18n */
const pi = useMaterialRequirementsPlanningI18n()

import type { MaterialRequirementsPlanningCreate } from '@/types/logistics/manufacturing/mrp/material-requirements-planning'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","materialRequirementsPlanningCode","masterProductionScheduleId","mpsCode","masterDemandScheduleId","mdsCode","planDate","planPeriodStart","planPeriodEnd","plannerId","planBy","runStatus","productionPlanId","productionPlanCode","purchasePlanId","purchasePlanCode","planDescription","extField","remark"]


import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'
import { resolveNextDetailLineNumber } from '@/utils/takt-sequence'
import { useMaterialRequirementsPlanningItemI18n } from '../composables/use-material-requirements-planning-item-i18n'

const materialRequirementsPlanningItemPi = useMaterialRequirementsPlanningItemI18n()

/** 弹窗/表格内 TaktSelect 下拉挂载容器（避免 overflow 裁剪与表头列错位） */
function getSelectPopupContainer(triggerNode?: HTMLElement): HTMLElement {
  return triggerNode?.ownerDocument?.body ?? document.body
}

const childMaterialRequirementsPlanningItemRows = ref<Record<string, unknown>[]>([])
const materialRequirementsPlanningItemTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 是否已持久化的子表行 */
function isPersistedMaterialRequirementsPlanningItemRow(row: Record<string, unknown>): boolean {
  const id = row.materialRequirementsPlanningItemId
  if (id == null || id === '') {
    return false
  }
  return String(id) !== '0'
}

/** 分配下一可用子表行号（含作废行，仅据当前表格行递增） */
function allocateNextMaterialRequirementsPlanningItemLineNumber(): number {
  const rows = materialRequirementsPlanningItemTableRef.value?.getRows?.() ?? childMaterialRequirementsPlanningItemRows.value
  return resolveNextDetailLineNumber(0, rows)
}

/** 子表 materialRequirementsPlanningItem 可编辑列 */
const materialRequirementsPlanningItemFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'lineNumber',
    title: materialRequirementsPlanningItemPi.label('lineNumber'),
    width: 140,
  },
  {
    key: 'materialCode',
    title: materialRequirementsPlanningItemPi.label('materialCode'),
    width: 140,
  },
  {
    key: 'materialName',
    title: materialRequirementsPlanningItemPi.label('materialName'),
    editor: 'input',
    width: 140,
  },
  {
    key: 'materialSpecification',
    title: materialRequirementsPlanningItemPi.label('materialSpecification'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialRequirementsPlanningItemPi.ph('materialSpecification'),
  },
  {
    key: 'modelCode',
    title: materialRequirementsPlanningItemPi.label('modelCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialRequirementsPlanningItemPi.ph('modelCode'),
  },
  {
    key: 'modelName',
    title: materialRequirementsPlanningItemPi.label('modelName'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialRequirementsPlanningItemPi.ph('modelName'),
  },
  {
    key: 'parentMaterialCode',
    title: materialRequirementsPlanningItemPi.label('parentMaterialCode'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: materialRequirementsPlanningItemPi.ph('parentMaterialCode'),
  },
  {
    key: 'bomLevel',
    title: materialRequirementsPlanningItemPi.label('bomLevel'),
    width: 140,
  },
  {
    key: 'requirementDate',
    title: materialRequirementsPlanningItemPi.label('requirementDate'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD',
    width: 140,
  },
  {
    key: 'planUnit',
    title: materialRequirementsPlanningItemPi.label('planUnit'),
    width: 140,
  },
  {
    key: 'grossRequirement',
    title: materialRequirementsPlanningItemPi.label('grossRequirement'),
    width: 140,
  },
  {
    key: 'scheduledReceipts',
    title: materialRequirementsPlanningItemPi.label('scheduledReceipts'),
    width: 140,
  },
  {
    key: 'onHandQuantity',
    title: materialRequirementsPlanningItemPi.label('onHandQuantity'),
    width: 140,
  },
  {
    key: 'projectedOnHand',
    title: materialRequirementsPlanningItemPi.label('projectedOnHand'),
    width: 140,
  },
  {
    key: 'netRequirement',
    title: materialRequirementsPlanningItemPi.label('netRequirement'),
    width: 140,
  },
  {
    key: 'procurementType',
    title: materialRequirementsPlanningItemPi.label('procurementType'),
    width: 140,
  },
  {
    key: 'isObsolete',
    title: materialRequirementsPlanningItemPi.label('isObsolete'),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<MaterialRequirementsPlanningCreate & { materialRequirementsPlanningId?: string }> | null | undefined) {
  const rows_materialRequirementsPlanningItem = ((val as any)?.items ?? []) as Record<string, unknown>[]
  childMaterialRequirementsPlanningItemRows.value = rows_materialRequirementsPlanningItem
}

function createDefaultMaterialRequirementsPlanningItemRow(): Record<string, unknown> {
  return {
    lineNumber: allocateNextMaterialRequirementsPlanningItemLineNumber(),
    materialCode: '',
    materialName: '',
    materialSpecification: '',
    modelCode: '',
    modelName: '',
    parentMaterialCode: '',
    bomLevel: 0,
    requirementDate: '',
    planUnit: '',
    grossRequirement: 0,
    scheduledReceipts: 0,
    onHandQuantity: 0,
    projectedOnHand: 0,
    netRequirement: 0,
    procurementType: 0,
    isObsolete: 0,
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.materialRequirementsPlanningId ?? ''
  const isUpdate = Boolean(masterId)
  return {
    ...formState,
    items: materialRequirementsPlanningItemTableRef.value?.getRows?.() ?? childMaterialRequirementsPlanningItemRows.value.map((row) => {
      const normalized = {
        ...row,
        tenantCode: tenantStore.tenantCode,
        companyCode: tenantStore.companyCode,
        companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
        materialRequirementsPlanningId: masterId,
      }
      if (isUpdate && isPersistedMaterialRequirementsPlanningItemRow(row)) {
        normalized.materialRequirementsPlanningItemId = row.materialRequirementsPlanningItemId
      } else {
        delete normalized.materialRequirementsPlanningItemId
      }
      return normalized
    }),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaterialRequirementsPlanningCreate & { materialRequirementsPlanningId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 materialRequirementsPlanningId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.materialRequirementsPlanningId) {
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
    const isCreate = !props.formData?.materialRequirementsPlanningId
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
  materialRequirementsPlanningCode: [
    {
      required: true,
      message: pi.ph('materialRequirementsPlanningCode'),
      trigger: 'blur'
    }
  ],
  planDate: [
    {
      required: true,
      message: pi.ph('planDate'),
      trigger: 'change'
    }
  ],
  planPeriodStart: [
    {
      required: true,
      message: pi.ph('planPeriodStart'),
      trigger: 'change'
    }
  ],
  planPeriodEnd: [
    {
      required: true,
      message: pi.ph('planPeriodEnd'),
      trigger: 'change'
    }
  ],
  planBy: [
    {
      required: true,
      message: pi.ph('planBy'),
      trigger: 'change'
    }
  ],
  runStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('runStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('runStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await materialRequirementsPlanningItemTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('runStatus' in payload) {
    const rawrunStatus = payload.runStatus
    payload.runStatus = typeof rawrunStatus === 'number' ? rawrunStatus : Number(rawrunStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.materialRequirementsPlanningId)
  childMaterialRequirementsPlanningItemRows.value = []
  materialRequirementsPlanningItemTableRef.value?.resetRows?.()
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
