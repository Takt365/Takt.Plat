<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/production-order/components -->
<!-- 文件名称：production-order-form.vue -->
<!-- 功能描述：生产工单实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="production-order-form-tabs"
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
                :label="t('entity.productionorder.plantcode')"
                name="plantCode"
              >
                <TaktSelect
                  v-model:value="formState.plantCode"
                  api-url="TaktPlants/options"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productionorder.plantcode') })"
                  :disabled="!!formData?.productionOrderId || loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionorder.prodordertype')"
                name="prodOrderType"
              >
                <TaktSelect
                  v-model:value="formState.prodOrderType"
                  dict-type="logistics_prod_order_type"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productionorder.prodordertype') })"
                  :disabled="loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionorder.prodordercode')"
                name="prodOrderCode"
              >
                <a-input
                  v-model:value="formState.prodOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionorder.prodordercode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.productionOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionorder.materialcode')"
                name="materialCode"
              >
                <TaktSelect
                  v-model:value="formState.materialCode"
                  api-url="TaktMaterials/options"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productionorder.materialcode') })"
                  :disabled="!!formData?.productionOrderId || loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionorder.prodorderqty')"
                name="prodOrderQty"
              >
                <a-input-number
                  v-model:value="formState.prodOrderQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionorder.prodorderqty') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionorder.producedqty')"
                name="producedQty"
              >
                <a-input-number
                  v-model:value="formState.producedQty"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionorder.producedqty') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionorder.unitofmeasure')"
                name="unitOfMeasure"
              >
                <TaktSelect
                  v-model:value="formState.unitOfMeasure"
                  dict-type="logistics_unit_of_measure_code"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productionorder.unitofmeasure') })"
                  :disabled="loading"
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
                :label="t('entity.productionorder.actualstartdate')"
                name="actualStartDate"
              >
                <a-date-picker
                  v-model:value="formState.actualStartDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productionorder.actualstartdate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionorder.actualenddate')"
                name="actualEndDate"
              >
                <a-date-picker
                  v-model:value="formState.actualEndDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productionorder.actualenddate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionorder.priority')"
                name="priority"
              >
                <TaktSelect
                  v-model:value="formState.priority"
                  dict-type="sys_priority_level_category"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productionorder.priority') })"
                  :disabled="loading"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionorder.workcenter')"
                name="workCenter"
              >
                <TaktSelect
                  v-model:value="formState.workCenter"
                  :options="filteredWorkCenterOptions"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productionorder.workcenter') })"
                  :disabled="loading || !formState.plantCode"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionorder.prodbatch')"
                name="prodBatch"
              >
                <a-input
                  v-model:value="formState.prodBatch"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionorder.prodbatch') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionorder.serialno')"
                name="serialNo"
              >
                <a-input
                  v-model:value="formState.serialNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionorder.serialno') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionorder.routingcode')"
                name="routingCode"
              >
                <a-input
                  v-model:value="formState.routingCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionorder.routingcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.productionOrderId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionorder.plannedorderid')"
                name="plannedOrderId"
              >
                <TaktSelect
                  v-model:value="formState.plannedOrderId"
                  :options="filteredPlannedOrderOptions"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productionorder.plannedorderid') })"
                  :disabled="loading || !formState.plantCode"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.productionorder.apsorderid')"
                name="apsOrderId"
              >
                <TaktSelect
                  v-model:value="formState.apsOrderId"
                  :options="filteredApsOrderOptions"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productionorder.apsorderid') })"
                  :disabled="loading || !formState.plantCode"
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
                :label="t('entity.productionorder.plannedstarttime')"
                name="plannedStartTime"
              >
                <a-date-picker
                  v-model:value="formState.plannedStartTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionorder.plannedstarttime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.productionorder.plannedendtime')"
                name="plannedEndTime"
              >
                <a-date-picker
                  v-model:value="formState.plannedEndTime"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.productionorder.plannedendtime') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.productionorder.status')"
                name="productionOrderStatus"
              >
                <TaktSelect
                  v-model:value="formState.productionOrderStatus"
                  dict-type="logistics_prod_status"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.productionorder.status') })"
                  :disabled="loading"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * 生产工单实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/manufacturing/output/production-order/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { ProductionOrderCreate } from '@/types/logistics/manufacturing/output/production-order'
import type { TaktSelectOption } from '@/types/common'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { getWorkCenterOptions } from '@/api/logistics/manufacturing/scheduling/work-center'
import { getPlannedOrderOptions } from '@/api/logistics/manufacturing/planning/planned-order'
import { getApsOrderOptions } from '@/api/logistics/manufacturing/scheduling/aps-order'

/** i18n 翻译函数 */
const { t } = useI18n()

/** Pinia：字典缓存 */
const dictDataStore = useDictDataStore()
/** 工作中心下拉全量选项 */
const workCenterOptions = ref<TaktSelectOption[]>([])
/** 计划订单下拉全量选项 */
const plannedOrderOptions = ref<TaktSelectOption[]>([])
/** APS 订单下拉全量选项 */
const apsOrderOptions = ref<TaktSelectOption[]>([])
/** 按当前工厂过滤的工作中心选项 */
const filteredWorkCenterOptions = computed(() => {
  const plantCode = formState.plantCode
  if (!plantCode) {
    return []
  }
  return workCenterOptions.value.filter((item) => String(item.extValue ?? '') === String(plantCode))
})
/** 按当前工厂过滤的计划订单选项 */
const filteredPlannedOrderOptions = computed(() => {
  const plantCode = formState.plantCode
  if (!plantCode) {
    return []
  }
  return plannedOrderOptions.value.filter((item) => String(item.extValue ?? '') === String(plantCode))
})
/** 按工厂与已选计划订单过滤的 APS 订单选项 */
const filteredApsOrderOptions = computed(() => {
  const plantCode = formState.plantCode
  if (!plantCode) {
    return []
  }
  return apsOrderOptions.value.filter((item) => {
    if (String(item.extValue ?? '') !== String(plantCode)) {
      return false
    }
    if (formState.plannedOrderId && item.extLabel) {
      return String(item.extLabel) === String(formState.plannedOrderId)
    }
    return true
  })
})

/** 加载工作中心选项 */
async function loadWorkCenterOptions() {
  workCenterOptions.value = await getWorkCenterOptions()
}

/** 加载计划订单选项 */
async function loadPlannedOrderOptions() {
  plannedOrderOptions.value = await getPlannedOrderOptions()
}

/** 加载 APS 订单选项 */
async function loadApsOrderOptions() {
  apsOrderOptions.value = await getApsOrderOptions()
}

/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()

/** 表单挂载时预加载字典与班组选项 */
onMounted(async () => {
  void dictDataStore.loadAllDictDataAsync()
  await Promise.all([
    loadWorkCenterOptions(),
    loadPlannedOrderOptions(),
    loadApsOrderOptions(),
  ])
})

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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","prodOrderType","prodOrderCode","materialCode","prodOrderQty","producedQty","unitOfMeasure","actualStartDate","actualEndDate","priority","workCenter","prodBatch","serialNo","routingCode","plannedOrderId","apsOrderId","plannedStartTime","plannedEndTime","productionOrderStatus","extField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ProductionOrderCreate & { productionOrderId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 productionOrderId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.productionOrderId) {
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
    const isCreate = !props.formData?.productionOrderId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 工厂变更时清理无效工作中心与计划/APS 订单 */
watch(
  () => formState.plantCode,
  (plantCode, prevPlantCode) => {
    if (props.formData?.productionOrderId) {
      return
    }
    if (!plantCode) {
      formState.workCenter = undefined
      formState.plannedOrderId = undefined
      formState.apsOrderId = undefined
      return
    }
    if (prevPlantCode && prevPlantCode !== plantCode) {
      if (formState.workCenter) {
        const wcStillValid = filteredWorkCenterOptions.value.some(
          (item) => String(item.dictValue ?? '') === String(formState.workCenter)
        )
        if (!wcStillValid) {
          formState.workCenter = undefined
        }
      }
      if (formState.plannedOrderId) {
        const plannedStillValid = filteredPlannedOrderOptions.value.some(
          (item) => String(item.dictValue ?? '') === String(formState.plannedOrderId)
        )
        if (!plannedStillValid) {
          formState.plannedOrderId = undefined
        }
      }
      if (formState.apsOrderId) {
        const apsStillValid = filteredApsOrderOptions.value.some(
          (item) => String(item.dictValue ?? '') === String(formState.apsOrderId)
        )
        if (!apsStillValid) {
          formState.apsOrderId = undefined
        }
      }
    }
  },
)

/** 计划订单变更时清理无效 APS 订单 */
watch(
  () => formState.plannedOrderId,
  () => {
    if (props.formData?.productionOrderId) {
      return
    }
    if (!formState.apsOrderId) {
      return
    }
    const apsStillValid = filteredApsOrderOptions.value.some(
      (item) => String(item.dictValue ?? '') === String(formState.apsOrderId)
    )
    if (!apsStillValid) {
      formState.apsOrderId = undefined
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productionorder.plantcode') }),
      trigger: 'blur'
    }
  ],
  prodOrderType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productionorder.prodordertype') }),
      trigger: 'blur'
    }
  ],
  prodOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productionorder.prodordercode') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productionorder.materialcode') }),
      trigger: 'blur'
    }
  ],
  prodOrderQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.productionorder.prodorderqty') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.productionorder.prodorderqty') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  producedQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.productionorder.producedqty') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.productionorder.producedqty') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  unitOfMeasure: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.productionorder.unitofmeasure') }),
      trigger: 'blur'
    }
  ],
  priority: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.productionorder.priority') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.productionorder.priority') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  plannedStartTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.productionorder.plannedstarttime') }),
      trigger: 'change'
    }
  ],
  plannedEndTime: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.productionorder.plannedendtime') }),
      trigger: 'change'
    }
  ],
  productionOrderStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.productionorder.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.productionorder.status') }))
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
  if ('prodOrderQty' in payload) {
    const rawprodOrderQty = payload.prodOrderQty
    payload.prodOrderQty = typeof rawprodOrderQty === 'number' ? rawprodOrderQty : Number(rawprodOrderQty)
  }
  if ('producedQty' in payload) {
    const rawproducedQty = payload.producedQty
    payload.producedQty = typeof rawproducedQty === 'number' ? rawproducedQty : Number(rawproducedQty)
  }
  if ('priority' in payload) {
    const rawpriority = payload.priority
    payload.priority = typeof rawpriority === 'number' ? rawpriority : Number(rawpriority)
  }
  if ('productionOrderStatus' in payload) {
    const rawStatus = payload.productionOrderStatus
    payload.productionOrderStatus = typeof rawStatus === 'number' ? rawStatus : Number(rawStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.productionOrderId)

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
