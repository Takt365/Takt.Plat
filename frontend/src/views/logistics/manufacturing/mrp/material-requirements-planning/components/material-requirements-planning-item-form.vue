<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/mrp/material-requirements-planning/components -->
<!-- 文件名称：material-requirements-planning-item-form.vue -->
<!-- 功能描述：物料需求计划 MRP 头表子表 materialRequirementsPlanningItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form material-requirements-planning-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="material-requirements-planning-item-form-tabs"
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
                :label="pi.label('lineNumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="pi.ph('lineNumber')"
                  style="width: 100%"
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
                  api-url="TaktMaterialPlants/options"
                  :placeholder="pi.ph('materialCode')"
                  :disabled="!!formData?.materialRequirementsPlanningItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('modelCode')"
                name="modelCode"
              >
                <a-input
                  v-model:value="formState.modelCode"
                  :placeholder="pi.ph('modelCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialRequirementsPlanningItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('modelName')"
                name="modelName"
              >
                <a-input
                  v-model:value="formState.modelName"
                  :placeholder="pi.ph('modelName')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('parentMaterialCode')"
                name="parentMaterialCode"
              >
                <a-input
                  v-model:value="formState.parentMaterialCode"
                  :placeholder="pi.ph('parentMaterialCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialRequirementsPlanningItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bomLevel')"
                name="bomLevel"
              >
                <a-input-number
                  v-model:value="formState.bomLevel"
                  :placeholder="pi.ph('bomLevel')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('requirementDate')"
                name="requirementDate"
              >
                <a-date-picker
                  v-model:value="formState.requirementDate"
                  :placeholder="pi.ph('requirementDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('planUnit')"
                name="planUnit"
              >
                <TaktSelect
                  v-model:value="formState.planUnit"
                  dict-type="logistics_unit_of_measure_code"
                  :placeholder="pi.ph('planUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('grossRequirement')"
                name="grossRequirement"
              >
                <a-input-number
                  v-model:value="formState.grossRequirement"
                  :placeholder="pi.ph('grossRequirement')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scheduledReceipts')"
                name="scheduledReceipts"
              >
                <a-input-number
                  v-model:value="formState.scheduledReceipts"
                  :placeholder="pi.ph('scheduledReceipts')"
                  style="width: 100%"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('onHandQuantity')"
                name="onHandQuantity"
              >
                <a-input-number
                  v-model:value="formState.onHandQuantity"
                  :placeholder="pi.ph('onHandQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('projectedOnHand')"
                name="projectedOnHand"
              >
                <a-input-number
                  v-model:value="formState.projectedOnHand"
                  :placeholder="pi.ph('projectedOnHand')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('netRequirement')"
                name="netRequirement"
              >
                <a-input-number
                  v-model:value="formState.netRequirement"
                  :placeholder="pi.ph('netRequirement')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('procurementType')"
                name="procurementType"
              >
                <TaktSelect
                  v-model:value="formState.procurementType"
                  dict-type="logistics_procurement_type"
                  :placeholder="pi.ph('procurementType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isObsolete')"
                name="isObsolete"
              >
                <TaktSelect
                  v-model:value="formState.isObsolete"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isObsolete')"
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
 * 物料需求计划 MRP 头表子表 materialRequirementsPlanningItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/mrp/material-requirements-planning/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useMaterialRequirementsPlanningItemI18n } from '../composables/use-material-requirements-planning-item-i18n'

/** 实体字段 i18n */
const pi = useMaterialRequirementsPlanningItemI18n()

import type { MaterialRequirementsPlanningItemCreate } from '@/types/logistics/manufacturing/mrp/material-requirements-planning-item'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["lineNumber","materialCode","modelCode","modelName","parentMaterialCode","bomLevel","requirementDate","planUnit","grossRequirement","scheduledReceipts","onHandQuantity","projectedOnHand","netRequirement","procurementType","isObsolete"]

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaterialRequirementsPlanningItemCreate & { materialRequirementsPlanningItemId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
  /** 主表选中行 Id（Create/Update 提交时写入外键） */
  masterId?: string
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
  masterId: '',
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 materialRequirementsPlanningItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.materialRequirementsPlanningItemId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      applyFormDefaults(formState)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('lineNumber'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('lineNumber'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialCode: [
    {
      required: true,
      message: pi.ph('materialCode'),
      trigger: 'change'
    }
  ],
  bomLevel: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('bomLevel'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('bomLevel'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  requirementDate: [
    {
      required: true,
      message: pi.ph('requirementDate'),
      trigger: 'change'
    }
  ],
  planUnit: [
    {
      required: true,
      message: pi.ph('planUnit'),
      trigger: 'change'
    }
  ],
  grossRequirement: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('grossRequirement'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('grossRequirement'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  scheduledReceipts: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scheduledReceipts'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scheduledReceipts'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  onHandQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('onHandQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('onHandQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  projectedOnHand: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('projectedOnHand'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('projectedOnHand'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  netRequirement: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('netRequirement'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('netRequirement'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  procurementType: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('procurementType'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('procurementType'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isObsolete: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isObsolete'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isObsolete'))
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

/** 映射为 Create/Update DTO（含主表外键 materialRequirementsPlanningId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('bomLevel' in payload) {
    const rawbomLevel = payload.bomLevel
    payload.bomLevel = typeof rawbomLevel === 'number' ? rawbomLevel : Number(rawbomLevel)
  }
  if ('grossRequirement' in payload) {
    const rawgrossRequirement = payload.grossRequirement
    payload.grossRequirement = typeof rawgrossRequirement === 'number' ? rawgrossRequirement : Number(rawgrossRequirement)
  }
  if ('scheduledReceipts' in payload) {
    const rawscheduledReceipts = payload.scheduledReceipts
    payload.scheduledReceipts = typeof rawscheduledReceipts === 'number' ? rawscheduledReceipts : Number(rawscheduledReceipts)
  }
  if ('onHandQuantity' in payload) {
    const rawonHandQuantity = payload.onHandQuantity
    payload.onHandQuantity = typeof rawonHandQuantity === 'number' ? rawonHandQuantity : Number(rawonHandQuantity)
  }
  if ('projectedOnHand' in payload) {
    const rawprojectedOnHand = payload.projectedOnHand
    payload.projectedOnHand = typeof rawprojectedOnHand === 'number' ? rawprojectedOnHand : Number(rawprojectedOnHand)
  }
  if ('netRequirement' in payload) {
    const rawnetRequirement = payload.netRequirement
    payload.netRequirement = typeof rawnetRequirement === 'number' ? rawnetRequirement : Number(rawnetRequirement)
  }
  if ('procurementType' in payload) {
    const rawprocurementType = payload.procurementType
    payload.procurementType = typeof rawprocurementType === 'number' ? rawprocurementType : Number(rawprocurementType)
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    payload.isObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.materialRequirementsPlanningId = props.masterId
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
  applyFormDefaults(formState)
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
