<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/defect/pcba-inspection/components -->
<!-- 文件名称：pcba-inspection-detail-form.vue -->
<!-- 功能描述：PCBA检查日报实体 不良率子表 pcbaInspectionDetail 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form pcba-inspection-detail-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="pcba-inspection-detail-form-tabs"
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
                  :disabled="!!formData?.pcbaInspectionDetailId"
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
                :label="pi.label('pcbaBoardType')"
                name="pcbaBoardType"
              >
                <TaktSelect
                  v-model:value="formState.pcbaBoardType"
                  dict-type="logistics_pcba_function_category"
                  :placeholder="pi.ph('pcbaBoardType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('visualInspectionLine')"
                name="visualInspectionLine"
              >
                <TaktSelect
                  v-model:value="formState.visualInspectionLine"
                  dict-type="logistics_visual_inspection_line_category"
                  :placeholder="pi.ph('visualInspectionLine')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('aoiLine')"
                name="aoiLine"
              >
                <TaktSelect
                  v-model:value="formState.aoiLine"
                  dict-type="logistics_aoi_inspection_line_category"
                  :placeholder="pi.ph('aoiLine')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('bSideAssemblyDate')"
                name="bSideAssemblyDate"
              >
                <a-date-picker
                  v-model:value="formState.bSideAssemblyDate"
                  :placeholder="pi.ph('bSideAssemblyDate')"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('tSideAssemblyDate')"
                name="tSideAssemblyDate"
              >
                <a-date-picker
                  v-model:value="formState.tSideAssemblyDate"
                  :placeholder="pi.ph('tSideAssemblyDate')"
                  value-format="YYYY-MM-DD"
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
                :label="pi.label('inspectorName')"
                name="inspectorName"
              >
                <TaktSelect
                  v-model:value="formState.inspectorName"
                  api-url="TaktEmployees/options"
                  :placeholder="pi.ph('inspectorName')"
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
                :label="pi.label('inspectionQty')"
                name="inspectionQty"
              >
                <a-input-number
                  v-model:value="formState.inspectionQty"
                  :placeholder="pi.ph('inspectionQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('inspectionStatus')"
                name="inspectionStatus"
              >
                <TaktSelect
                  v-model:value="formState.inspectionStatus"
                  dict-type="logistics_pcba_inspection_status"
                  :placeholder="pi.ph('inspectionStatus')"
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
                :label="pi.label('inspectionWorkHours')"
                name="inspectionWorkHours"
              >
                <a-input-number
                  v-model:value="formState.inspectionWorkHours"
                  :placeholder="pi.ph('inspectionWorkHours')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('aoiWorkHours')"
                name="aoiWorkHours"
              >
                <a-input-number
                  v-model:value="formState.aoiWorkHours"
                  :placeholder="pi.ph('aoiWorkHours')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defectQty')"
                name="defectQty"
              >
                <a-input-number
                  v-model:value="formState.defectQty"
                  :placeholder="pi.ph('defectQty')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('handPlacement')"
                name="handPlacement"
              >
                <a-input
                  v-model:value="formState.handPlacement"
                  :placeholder="pi.ph('handPlacement')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('serialNumber')"
                name="serialNumber"
              >
                <a-input
                  v-model:value="formState.serialNumber"
                  :placeholder="pi.ph('serialNumber')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('content')"
                name="content"
              >
                <a-textarea
                  v-model:value="formState.content"
                  :placeholder="pi.ph('content')"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('defectLocation')"
                name="defectLocation"
              >
                <TaktSelect
                  v-model:value="formState.defectLocation"
                  dict-type="logistics_pcb_location_category"
                  :placeholder="pi.ph('defectLocation')"
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
 * PCBA检查日报实体 不良率子表 pcbaInspectionDetail 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/defect/pcba-inspection/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { usePcbaInspectionDetailI18n } from '../composables/use-pcba-inspection-detail-i18n'

/** 实体字段 i18n */
const pi = usePcbaInspectionDetailI18n()

import type { PcbaInspectionDetailCreate } from '@/types/logistics/manufacturing/defect/pcba-inspection-detail'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["prodOrderCode","lineNumber","pcbaBoardType","visualInspectionLine","aoiLine","bSideAssemblyDate","tSideAssemblyDate","shiftNo","inspectorName","dailyCompletedQty","inspectionQty","inspectionStatus","prodTeam","inspectionWorkHours","aoiWorkHours","defectQty","handPlacement","serialNumber","content","defectLocation","isObsolete"]



/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PcbaInspectionDetailCreate & { pcbaInspectionDetailId?: string }> | null
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  inspectionStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 pcbaInspectionDetailId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.pcbaInspectionDetailId) {
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
  inspectionQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('inspectionQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('inspectionQty'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inspectionStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('inspectionStatus'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('inspectionStatus'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  inspectionWorkHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('inspectionWorkHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('inspectionWorkHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  aoiWorkHours: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('aoiWorkHours'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('aoiWorkHours'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  defectQty: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('defectQty'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('defectQty'))
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

/** 映射为 Create/Update DTO（含主表外键 pcbaInspectionId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('shiftNo' in payload) {
    const rawshiftNo = payload.shiftNo
    payload.shiftNo = typeof rawshiftNo === 'number' ? rawshiftNo : Number(rawshiftNo)
  }
  if ('dailyCompletedQty' in payload) {
    const rawdailyCompletedQty = payload.dailyCompletedQty
    payload.dailyCompletedQty = typeof rawdailyCompletedQty === 'number' ? rawdailyCompletedQty : Number(rawdailyCompletedQty)
  }
  if ('inspectionQty' in payload) {
    const rawinspectionQty = payload.inspectionQty
    payload.inspectionQty = typeof rawinspectionQty === 'number' ? rawinspectionQty : Number(rawinspectionQty)
  }
  if ('inspectionStatus' in payload) {
    const rawinspectionStatus = payload.inspectionStatus
    payload.inspectionStatus = typeof rawinspectionStatus === 'number' ? rawinspectionStatus : Number(rawinspectionStatus)
  }
  if ('inspectionWorkHours' in payload) {
    const rawinspectionWorkHours = payload.inspectionWorkHours
    payload.inspectionWorkHours = typeof rawinspectionWorkHours === 'number' ? rawinspectionWorkHours : Number(rawinspectionWorkHours)
  }
  if ('aoiWorkHours' in payload) {
    const rawaoiWorkHours = payload.aoiWorkHours
    payload.aoiWorkHours = typeof rawaoiWorkHours === 'number' ? rawaoiWorkHours : Number(rawaoiWorkHours)
  }
  if ('defectQty' in payload) {
    const rawdefectQty = payload.defectQty
    payload.defectQty = typeof rawdefectQty === 'number' ? rawdefectQty : Number(rawdefectQty)
  }
  if ('isObsolete' in payload) {
    const rawisObsolete = payload.isObsolete
    payload.isObsolete = typeof rawisObsolete === 'number' ? rawisObsolete : Number(rawisObsolete)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.pcbaInspectionId = props.masterId
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
