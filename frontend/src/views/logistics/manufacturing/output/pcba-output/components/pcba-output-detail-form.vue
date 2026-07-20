<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/output/pcba-output/components -->
<!-- 文件名称：pcba-output-detail-form.vue -->
<!-- 功能描述：PCBA日报实体 达成率子表 pcbaOutputDetail 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form pcba-output-detail-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="pcba-output-detail-form-tabs"
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
                :label="pi.label('prodOrderCode')"
                name="prodOrderCode"
              >
                <a-input
                  v-model:value="formState.prodOrderCode"
                  :placeholder="pi.ph('prodOrderCode')"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.pcbaOutputDetailId"
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
                :label="pi.label('timePeriod')"
                name="timePeriod"
              >
                <a-input
                  v-model:value="formState.timePeriod"
                  :placeholder="pi.ph('timePeriod')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('prodTeam')"
                name="prodTeam"
              >
                <TaktSelect
                  v-model:value="formState.prodTeam"
                  api-url="TaktProductionTeams/options"
                  :placeholder="pi.ph('prodTeam')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('productionEquipmentCode')"
                name="productionEquipmentCode"
              >
                <TaktSelect
                  v-model:value="formState.productionEquipmentCode"
                  api-url="TaktProductionEquipments/options"
                  :placeholder="pi.ph('productionEquipmentCode')"
                  :disabled="!!formData?.pcbaOutputDetailId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('directLabor')"
                name="directLabor"
              >
                <a-input-number
                  v-model:value="formState.directLabor"
                  :placeholder="pi.ph('directLabor')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('indirectLabor')"
                name="indirectLabor"
              >
                <a-input-number
                  v-model:value="formState.indirectLabor"
                  :placeholder="pi.ph('indirectLabor')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('shiftNo')"
                name="shiftNo"
              >
                <TaktSelect
                  v-model:value="formState.shiftNo"
                  dict-type="logistics_shift_category"
                  :placeholder="pi.ph('shiftNo')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stdShorts')"
                name="stdShorts"
              >
                <a-input-number
                  v-model:value="formState.stdShorts"
                  :placeholder="pi.ph('stdShorts')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('pcbBoardType')"
                name="pcbBoardType"
              >
                <TaktSelect
                  v-model:value="formState.pcbBoardType"
                  dict-type="logistics_pcba_function_category"
                  :placeholder="pi.ph('pcbBoardType')"
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
                :label="pi.label('panelSide')"
                name="panelSide"
              >
                <TaktSelect
                  v-model:value="formState.panelSide"
                  dict-type="logistics_pcba_side_category"
                  :placeholder="pi.ph('panelSide')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('batchQty')"
                name="batchQty"
              >
                <a-input-number
                  v-model:value="formState.batchQty"
                  :placeholder="pi.ph('batchQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('dailyCompletedQty')"
                name="dailyCompletedQty"
              >
                <a-input-number
                  v-model:value="formState.dailyCompletedQty"
                  :placeholder="pi.ph('dailyCompletedQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serialNo')"
                name="serialNo"
              >
                <a-input
                  v-model:value="formState.serialNo"
                  :placeholder="pi.ph('serialNo')"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defectCount')"
                name="defectCount"
              >
                <a-input-number
                  v-model:value="formState.defectCount"
                  :placeholder="pi.ph('defectCount')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('downtimeMinutes')"
                name="downtimeMinutes"
              >
                <a-input-number
                  v-model:value="formState.downtimeMinutes"
                  :placeholder="pi.ph('downtimeMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('downtimeReason')"
                name="downtimeReason"
              >
                <TaktSelect
                  v-model:value="formState.downtimeReason"
                  dict-type="logistics_stop_reason_category"
                  :placeholder="pi.ph('downtimeReason')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('downtimeDescription')"
                name="downtimeDescription"
              >
                <a-textarea
                  v-model:value="formState.downtimeDescription"
                  :placeholder="pi.ph('downtimeDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('repairMinutes')"
                name="repairMinutes"
              >
                <a-input-number
                  v-model:value="formState.repairMinutes"
                  :placeholder="pi.ph('repairMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('switchCount')"
                name="switchCount"
              >
                <a-input-number
                  v-model:value="formState.switchCount"
                  :placeholder="pi.ph('switchCount')"
                  style="width: 100%"
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
            <a-col :span="12">
              <a-form-item
                :label="pi.label('switchTime')"
                name="switchTime"
              >
                <a-input-number
                  v-model:value="formState.switchTime"
                  :placeholder="pi.ph('switchTime')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('stopTime')"
                name="stopTime"
              >
                <a-input-number
                  v-model:value="formState.stopTime"
                  :placeholder="pi.ph('stopTime')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('totalMinutes')"
                name="totalMinutes"
              >
                <a-input-number
                  v-model:value="formState.totalMinutes"
                  :placeholder="pi.ph('totalMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('unachievedReason')"
                name="unachievedReason"
              >
                <TaktSelect
                  v-model:value="formState.unachievedReason"
                  dict-type="logistics_nonachievement_reason_category"
                  :placeholder="pi.ph('unachievedReason')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('unachievedDescription')"
                name="unachievedDescription"
              >
                <a-textarea
                  v-model:value="formState.unachievedDescription"
                  :placeholder="pi.ph('unachievedDescription')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('confirmMinutes')"
                name="confirmMinutes"
              >
                <a-input-number
                  v-model:value="formState.confirmMinutes"
                  :placeholder="pi.ph('confirmMinutes')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('mixedProd')"
                name="mixedProd"
              >
                <a-input-number
                  v-model:value="formState.mixedProd"
                  :placeholder="pi.ph('mixedProd')"
                  style="width: 100%"
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
 * PCBA日报实体 达成率子表 pcbaOutputDetail 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/output/pcba-output/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePcbaOutputDetailI18n } from '../composables/use-pcba-output-detail-i18n'
import { usePcbaOutputDetailDictFormat } from '../composables/use-pcba-output-detail-dict-format'

/** 实体字段 i18n */
const pi = usePcbaOutputDetailI18n()
const { hydrateDetailDictFields, formatDetailDictFieldsForSubmit } = usePcbaOutputDetailDictFormat()

import type { PcbaOutputDetailCreate } from '@/types/logistics/manufacturing/output/pcba-output-detail'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["prodOrderCode","lineNumber","timePeriod","prodTeam","productionEquipmentCode","directLabor","indirectLabor","shiftNo","stdShorts","pcbBoardType","panelSide","batchQty","dailyCompletedQty","serialNo","defectCount","downtimeMinutes","downtimeReason","downtimeDescription","repairMinutes","switchCount","switchTime","stopTime","totalMinutes","unachievedReason","unachievedDescription","confirmMinutes","mixedProd","isObsolete"]



/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PcbaOutputDetailCreate & { pcbaOutputDetailId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 pcbaOutputDetailId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.pcbaOutputDetailId) {
      const next = { ...val } as Record<string, unknown>
      hydrateDetailDictFields(next)
      Object.keys(formState).forEach((k) => delete formState[k])

      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        const next = { ...val } as Record<string, unknown>
        hydrateDetailDictFields(next)
        Object.assign(formState, next)
      }
      applyFormDefaults(formState)
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  prodOrderCode: [
    {
      required: true,
      message: pi.ph('prodOrderCode'),
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
  timePeriod: [
    {
      required: true,
      message: pi.ph('timePeriod'),
      trigger: 'blur'
    }
  ],
  prodTeam: [
    {
      required: true,
      message: pi.ph('prodTeam'),
      trigger: 'change'
    }
  ],
  productionEquipmentCode: [
    {
      required: true,
      message: pi.ph('productionEquipmentCode'),
      trigger: 'change'
    }
  ],
  directLabor: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('directLabor'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('directLabor'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  indirectLabor: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('indirectLabor'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('indirectLabor'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  shiftNo: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('shiftNo'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('shiftNo'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  stdShorts: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('stdShorts'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('stdShorts'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  pcbBoardType: [
    {
      required: true,
      message: pi.ph('pcbBoardType'),
      trigger: 'change'
    }
  ],
  panelSide: [
    {
      required: true,
      message: pi.ph('panelSide'),
      trigger: 'change'
    }
  ],
  batchQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('batchQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('batchQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  dailyCompletedQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('dailyCompletedQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('dailyCompletedQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  serialNo: [
    {
      required: true,
      message: pi.ph('serialNo'),
      trigger: 'blur'
    }
  ],
  defectCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('defectCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('defectCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  downtimeMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('downtimeMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('downtimeMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  repairMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('repairMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('repairMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  switchCount: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('switchCount'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('switchCount'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  switchTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('switchTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('switchTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  stopTime: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('stopTime'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('stopTime'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  totalMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('totalMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('totalMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  confirmMinutes: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('confirmMinutes'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('confirmMinutes'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  mixedProd: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('mixedProd'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('mixedProd'))
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

/** 映射为 Create/Update DTO（含主表外键 pcbaOutputId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('directLabor' in payload) {
    const rawdirectLabor = payload.directLabor
    payload.directLabor = typeof rawdirectLabor === 'number' ? rawdirectLabor : Number(rawdirectLabor)
  }
  if ('indirectLabor' in payload) {
    const rawindirectLabor = payload.indirectLabor
    payload.indirectLabor = typeof rawindirectLabor === 'number' ? rawindirectLabor : Number(rawindirectLabor)
  }
  if ('shiftNo' in payload) {
    const rawshiftNo = payload.shiftNo
    payload.shiftNo = typeof rawshiftNo === 'number' ? rawshiftNo : Number(rawshiftNo)
  }
  if ('stdShorts' in payload) {
    const rawstdShorts = payload.stdShorts
    payload.stdShorts = typeof rawstdShorts === 'number' ? rawstdShorts : Number(rawstdShorts)
  }
  if ('batchQty' in payload) {
    const rawbatchQty = payload.batchQty
    payload.batchQty = typeof rawbatchQty === 'number' ? rawbatchQty : Number(rawbatchQty)
  }
  if ('dailyCompletedQty' in payload) {
    const rawdailyCompletedQty = payload.dailyCompletedQty
    payload.dailyCompletedQty = typeof rawdailyCompletedQty === 'number' ? rawdailyCompletedQty : Number(rawdailyCompletedQty)
  }
  if ('defectCount' in payload) {
    const rawdefectCount = payload.defectCount
    payload.defectCount = typeof rawdefectCount === 'number' ? rawdefectCount : Number(rawdefectCount)
  }
  if ('downtimeMinutes' in payload) {
    const rawdowntimeMinutes = payload.downtimeMinutes
    payload.downtimeMinutes = typeof rawdowntimeMinutes === 'number' ? rawdowntimeMinutes : Number(rawdowntimeMinutes)
  }
  if ('repairMinutes' in payload) {
    const rawrepairMinutes = payload.repairMinutes
    payload.repairMinutes = typeof rawrepairMinutes === 'number' ? rawrepairMinutes : Number(rawrepairMinutes)
  }
  if ('switchCount' in payload) {
    const rawswitchCount = payload.switchCount
    payload.switchCount = typeof rawswitchCount === 'number' ? rawswitchCount : Number(rawswitchCount)
  }
  if ('switchTime' in payload) {
    const rawswitchTime = payload.switchTime
    payload.switchTime = typeof rawswitchTime === 'number' ? rawswitchTime : Number(rawswitchTime)
  }
  if ('stopTime' in payload) {
    const rawstopTime = payload.stopTime
    payload.stopTime = typeof rawstopTime === 'number' ? rawstopTime : Number(rawstopTime)
  }
  if ('totalMinutes' in payload) {
    const rawtotalMinutes = payload.totalMinutes
    payload.totalMinutes = typeof rawtotalMinutes === 'number' ? rawtotalMinutes : Number(rawtotalMinutes)
  }
  if ('confirmMinutes' in payload) {
    const rawconfirmMinutes = payload.confirmMinutes
    payload.confirmMinutes = typeof rawconfirmMinutes === 'number' ? rawconfirmMinutes : Number(rawconfirmMinutes)
  }
  if ('mixedProd' in payload) {
    const rawmixedProd = payload.mixedProd
    payload.mixedProd = typeof rawmixedProd === 'number' ? rawmixedProd : Number(rawmixedProd)
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    payload.isObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  formatDetailDictFieldsForSubmit(payload)
  payload.pcbaOutputId = props.masterId
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    const next = { ...props.formData } as Record<string, unknown>
    hydrateDetailDictFields(next)
    Object.assign(formState, next)
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
