<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/code/generator/gen-table/components -->
<!-- 文件名称：gen-table-column-form.vue -->
<!-- 功能描述：Takt代码生成表配置实体 特例：继承组合 4：无关联工厂、无语言子表 genTableColumn 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form gen-table-column-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="gen-table-column-form-tabs"
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
                :label="pi.label('databaseColumnName')"
                name="databaseColumnName"
              >
                <a-input
                  v-model:value="formState.databaseColumnName"
                  :placeholder="pi.ph('databaseColumnName')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('columnComment')"
                name="columnComment"
              >
                <a-input
                  v-model:value="formState.columnComment"
                  :placeholder="pi.ph('columnComment')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('databaseDataType')"
                name="databaseDataType"
              >
                <TaktSelect
                  v-model:value="formState.databaseDataType"
                  dict-type="sys_db_data_type"
                  :placeholder="pi.ph('databaseDataType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('csharpDataType')"
                name="csharpDataType"
              >
                <TaktSelect
                  v-model:value="formState.csharpDataType"
                  dict-type="gen_csharp_data_type"
                  :placeholder="pi.ph('csharpDataType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('csharpColumnName')"
                name="csharpColumnName"
              >
                <a-input
                  v-model:value="formState.csharpColumnName"
                  :placeholder="pi.ph('csharpColumnName')"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('length')"
                name="length"
              >
                <a-input-number
                  v-model:value="formState.length"
                  :placeholder="pi.ph('length')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('decimalDigits')"
                name="decimalDigits"
              >
                <a-input-number
                  v-model:value="formState.decimalDigits"
                  :placeholder="pi.ph('decimalDigits')"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isPk')"
                name="isPk"
              >
                <TaktSelect
                  v-model:value="formState.isPk"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isPk')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isIncrement')"
                name="isIncrement"
              >
                <TaktSelect
                  v-model:value="formState.isIncrement"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isIncrement')"
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
                :label="pi.label('isRequired')"
                name="isRequired"
              >
                <TaktSelect
                  v-model:value="formState.isRequired"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isRequired')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isCreate')"
                name="isCreate"
              >
                <TaktSelect
                  v-model:value="formState.isCreate"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isCreate')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isUpdate')"
                name="isUpdate"
              >
                <TaktSelect
                  v-model:value="formState.isUpdate"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isUpdate')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isUnique')"
                name="isUnique"
              >
                <TaktSelect
                  v-model:value="formState.isUnique"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isUnique')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isList')"
                name="isList"
              >
                <TaktSelect
                  v-model:value="formState.isList"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isList')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isExport')"
                name="isExport"
              >
                <TaktSelect
                  v-model:value="formState.isExport"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isExport')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isSort')"
                name="isSort"
              >
                <TaktSelect
                  v-model:value="formState.isSort"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isSort')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('isQuery')"
                name="isQuery"
              >
                <TaktSelect
                  v-model:value="formState.isQuery"
                  dict-type="sys_yes_no_type"
                  :placeholder="pi.ph('isQuery')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('queryType')"
                name="queryType"
              >
                <TaktSelect
                  v-model:value="formState.queryType"
                  dict-type="gen_query_type"
                  :placeholder="pi.ph('queryType')"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="pi.label('htmlType')"
                name="htmlType"
              >
                <TaktSelect
                  v-model:value="formState.htmlType"
                  dict-type="gen_display_type"
                  :placeholder="pi.ph('htmlType')"
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
                :label="pi.label('dictType')"
                name="dictType"
              >
                <TaktSelect
                  v-model:value="formState.dictType"
                  api-url="TaktDictTypes/options"
                  :placeholder="pi.ph('dictType')"
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
 * Takt代码生成表配置实体 特例：继承组合 4：无关联工厂、无语言子表 genTableColumn 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/code/generator/gen-table/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import { useGenTableColumnI18n } from '../composables/use-gen-table-column-i18n'

/** 实体字段 i18n */
const pi = useGenTableColumnI18n()

import type { GenTableColumnCreate } from '@/types/code/generator/gen-table-column'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["lineNumber","databaseColumnName","columnComment","databaseDataType","csharpDataType","csharpColumnName","length","decimalDigits","isPk","isIncrement","isRequired","isCreate","isUpdate","isUnique","isList","isExport","isSort","isQuery","queryType","htmlType","dictType"]



/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<GenTableColumnCreate & { genTableColumnId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 genTableColumnId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.genTableColumnId) {
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
  databaseColumnName: [
    {
      required: true,
      message: pi.ph('databaseColumnName'),
      trigger: 'blur'
    }
  ],
  databaseDataType: [
    {
      required: true,
      message: pi.ph('databaseDataType'),
      trigger: 'change'
    }
  ],
  csharpDataType: [
    {
      required: true,
      message: pi.ph('csharpDataType'),
      trigger: 'change'
    }
  ],
  csharpColumnName: [
    {
      required: true,
      message: pi.ph('csharpColumnName'),
      trigger: 'blur'
    }
  ],
  length: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('length'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('length'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  decimalDigits: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('decimalDigits'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('decimalDigits'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isPk: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isPk'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isPk'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isIncrement: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isIncrement'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isIncrement'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isRequired: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isRequired'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isRequired'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isCreate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isCreate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isCreate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isUpdate: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isUpdate'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isUpdate'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isUnique: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isUnique'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isUnique'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isList: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isList'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isList'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isExport: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isExport'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isExport'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isSort: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isSort'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isSort'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  isQuery: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(pi.ph('isQuery'))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(pi.ph('isQuery'))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  queryType: [
    {
      required: true,
      message: pi.ph('queryType'),
      trigger: 'change'
    }
  ],
  htmlType: [
    {
      required: true,
      message: pi.ph('htmlType'),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 genTableId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('length' in payload) {
    const rawlength = payload.length
    payload.length = typeof rawlength === 'number' ? rawlength : Number(rawlength)
  }
  if ('decimalDigits' in payload) {
    const rawdecimalDigits = payload.decimalDigits
    payload.decimalDigits = typeof rawdecimalDigits === 'number' ? rawdecimalDigits : Number(rawdecimalDigits)
  }
  if ('isPk' in payload) {
    const rawisPk = payload.isPk
    payload.isPk = typeof rawisPk === 'number' ? rawisPk : Number(rawisPk)
  }
  if ('isIncrement' in payload) {
    const rawisIncrement = payload.isIncrement
    payload.isIncrement = typeof rawisIncrement === 'number' ? rawisIncrement : Number(rawisIncrement)
  }
  if ('isRequired' in payload) {
    const rawisRequired = payload.isRequired
    payload.isRequired = typeof rawisRequired === 'number' ? rawisRequired : Number(rawisRequired)
  }
  if ('isCreate' in payload) {
    const rawisCreate = payload.isCreate
    payload.isCreate = typeof rawisCreate === 'number' ? rawisCreate : Number(rawisCreate)
  }
  if ('isUpdate' in payload) {
    const rawisUpdate = payload.isUpdate
    payload.isUpdate = typeof rawisUpdate === 'number' ? rawisUpdate : Number(rawisUpdate)
  }
  if ('isUnique' in payload) {
    const rawisUnique = payload.isUnique
    payload.isUnique = typeof rawisUnique === 'number' ? rawisUnique : Number(rawisUnique)
  }
  if ('isList' in payload) {
    const rawisList = payload.isList
    payload.isList = typeof rawisList === 'number' ? rawisList : Number(rawisList)
  }
  if ('isExport' in payload) {
    const rawisExport = payload.isExport
    payload.isExport = typeof rawisExport === 'number' ? rawisExport : Number(rawisExport)
  }
  if ('isSort' in payload) {
    const rawisSort = payload.isSort
    payload.isSort = typeof rawisSort === 'number' ? rawisSort : Number(rawisSort)
  }
  if ('isQuery' in payload) {
    const rawisQuery = payload.isQuery
    payload.isQuery = typeof rawisQuery === 'number' ? rawisQuery : Number(rawisQuery)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.genTableId = props.masterId
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
