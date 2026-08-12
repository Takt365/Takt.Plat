<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material/components -->
<!-- 文件名称：bill-of-material-item-form.vue -->
<!-- 功能描述：Takt物料清单实体子表 billOfMaterialItem 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form bill-of-material-item-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="bill-of-material-item-form-tabs"
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
                :label="pi.label('bomCode')"
                name="bomCode"
              >
                <a-input
                  v-model:value="formState.bomCode"
                  :placeholder="pi.ph('bomCode')"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.billOfMaterialItemId"
                />
              </a-form-item>
            </a-col>
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
                  :disabled="!!formData?.billOfMaterialItemId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('usageQuantity')"
                name="usageQuantity"
              >
                <a-input-number
                  v-model:value="formState.usageQuantity"
                  :placeholder="pi.ph('usageQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('materialUnit')"
                name="materialUnit"
              >
                <TaktSelect
                  v-model:value="formState.materialUnit"
                  dict-type="logistics_unit_of_measure_code"
                  :placeholder="pi.ph('materialUnit')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('scrapRate')"
                name="scrapRate"
              >
                <a-input-number
                  v-model:value="formState.scrapRate"
                  :placeholder="pi.ph('scrapRate')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('actualUsageQuantity')"
                name="actualUsageQuantity"
              >
                <a-input-number
                  v-model:value="formState.actualUsageQuantity"
                  :placeholder="pi.ph('actualUsageQuantity')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('operationSeq')"
                name="operationSeq"
              >
                <a-input-number
                  v-model:value="formState.operationSeq"
                  :placeholder="pi.ph('operationSeq')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('workCenter')"
                name="workCenter"
              >
                <TaktSelect
                  v-model:value="formState.workCenter"
                  api-url="TaktWorkCenters/options"
                  :placeholder="pi.ph('workCenter')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('position')"
                name="position"
              >
                <a-input
                  v-model:value="formState.position"
                  :placeholder="pi.ph('position')"
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
        key="tab-1"
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="pi.label('substituteGroup')"
                name="substituteGroup"
              >
                <a-input
                  v-model:value="formState.substituteGroup"
                  :placeholder="pi.ph('substituteGroup')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('substitutePriority')"
                name="substitutePriority"
              >
                <a-input-number
                  v-model:value="formState.substitutePriority"
                  :placeholder="pi.ph('substitutePriority')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isOptional')"
                name="isOptional"
              >
                <TaktSelect
                  v-model:value="formState.isOptional"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isOptional')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isPhantom')"
                name="isPhantom"
              >
                <TaktSelect
                  v-model:value="formState.isPhantom"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isPhantom')"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('substitutes')"
                name="substitutes"
              >
                <a-input
                  v-model:value="formState.substitutes"
                  :placeholder="pi.ph('substitutes')"
                  show-count
                  :maxlength="20"
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
 * Takt物料清单实体子表 billOfMaterialItem 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/bom/bill-of-material/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useBillOfMaterialItemI18n } from '../composables/use-bill-of-material-item-i18n'

/** 实体字段 i18n */
const pi = useBillOfMaterialItemI18n()

import type { BillOfMaterialItemCreate } from '@/types/logistics/manufacturing/bom/bill-of-material-item'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["bomCode","lineNumber","materialCode","usageQuantity","materialUnit","scrapRate","actualUsageQuantity","operationSeq","workCenter","position","substituteGroup","substitutePriority","isOptional","isPhantom","isObsolete","substitutes"]



/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<BillOfMaterialItemCreate & { billOfMaterialItemId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 billOfMaterialItemId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.billOfMaterialItemId) {
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
  bomCode: [
    {
      required: true,
      message: pi.ph('bomCode'),
      trigger: 'blur'
    }
  ],
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
  usageQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('usageQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('usageQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialUnit: [
    {
      required: true,
      message: pi.ph('materialUnit'),
      trigger: 'change'
    }
  ],
  scrapRate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('scrapRate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('scrapRate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  actualUsageQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('actualUsageQuantity'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('actualUsageQuantity'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  operationSeq: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('operationSeq'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('operationSeq'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  substitutePriority: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('substitutePriority'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('substitutePriority'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isOptional: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isOptional'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isOptional'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isPhantom: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isPhantom'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isPhantom'))
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

/** 映射为 Create/Update DTO（含主表外键 billOfMaterialId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('usageQuantity' in payload) {
    const rawusageQuantity = payload.usageQuantity
    payload.usageQuantity = typeof rawusageQuantity === 'number' ? rawusageQuantity : Number(rawusageQuantity)
  }
  if ('scrapRate' in payload) {
    const rawscrapRate = payload.scrapRate
    payload.scrapRate = typeof rawscrapRate === 'number' ? rawscrapRate : Number(rawscrapRate)
  }
  if ('actualUsageQuantity' in payload) {
    const rawactualUsageQuantity = payload.actualUsageQuantity
    payload.actualUsageQuantity = typeof rawactualUsageQuantity === 'number' ? rawactualUsageQuantity : Number(rawactualUsageQuantity)
  }
  if ('operationSeq' in payload) {
    const rawoperationSeq = payload.operationSeq
    payload.operationSeq = typeof rawoperationSeq === 'number' ? rawoperationSeq : Number(rawoperationSeq)
  }
  if ('substitutePriority' in payload) {
    const rawsubstitutePriority = payload.substitutePriority
    payload.substitutePriority = typeof rawsubstitutePriority === 'number' ? rawsubstitutePriority : Number(rawsubstitutePriority)
  }
  if ('isOptional' in payload) {
    const rawisOptional = payload.isOptional
    payload.isOptional = typeof rawisOptional === 'number' ? rawisOptional : Number(rawisOptional)
  }
  if ('isPhantom' in payload) {
    const rawisPhantom = payload.isPhantom
    payload.isPhantom = typeof rawisPhantom === 'number' ? rawisPhantom : Number(rawisPhantom)
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    payload.isObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.billOfMaterialId = props.masterId
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
