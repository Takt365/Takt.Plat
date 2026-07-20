<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/engineering-change/ec-gijutsu/components -->
<!-- 文件名称：ec-detail-form.vue -->
<!-- 功能描述：设变子表 ecDetail 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form ec-detail-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="ec-detail-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecno')"
                name="ecNo"
              >
                <a-input
                  v-model:value="formState.ecNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecno') })"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecmodel')"
                name="ecModel"
              >
                <a-input
                  v-model:value="formState.ecModel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecmodel') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecbomitem')"
                name="ecBomItem"
              >
                <a-input
                  v-model:value="formState.ecBomItem"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecbomitem') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecbomitemtext')"
                name="ecBomItemText"
              >
                <a-input
                  v-model:value="formState.ecBomItemText"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecbomitemtext') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecbomsubitem')"
                name="ecBomSubItem"
              >
                <a-input
                  v-model:value="formState.ecBomSubItem"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecbomsubitem') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecbomsubitemtext')"
                name="ecBomSubItemText"
              >
                <a-input
                  v-model:value="formState.ecBomSubItemText"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecbomsubitemtext') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.isendofline')"
                name="isEndOfLine"
              >
                <TaktSelect
                  v-model:value="formState.isEndOfLine"
                  dict-type="logistics_material_eol_status"
                  allow-clear
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.isendofline') })"
                />
              </a-form-item>
            </a-col>
          </a-row>
        </div>
      </a-tab-pane>
      <a-tab-pane
        key="tab-1"
        :tab="t('logistics.manufacturing.engineering-change.ec-gijutsu.page.tabs.oldNewMaterial')"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecolditem')"
                name="ecOldItem"
              >
                <a-input
                  v-model:value="formState.ecOldItem"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecolditem') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecoldtext')"
                name="ecOldText"
              >
                <a-input
                  v-model:value="formState.ecOldText"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecoldtext') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecoldusage')"
                name="ecOldUsage"
              >
                <a-input-number
                  v-model:value="formState.ecOldUsage"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecoldusage') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecoldposition')"
                name="ecOldPosition"
              >
                <a-input
                  v-model:value="formState.ecOldPosition"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecoldposition') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecoldstock')"
                name="ecOldStock"
              >
                <a-input-number
                  v-model:value="formState.ecOldStock"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecoldstock') })"
                  :min="0"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecoldwarehouse')"
                name="ecOldWarehouse"
              >
                <TaktSelect
                  v-model:value="formState.ecOldWarehouse"
                  api-url="TaktWarehouses/options"
                  allow-clear
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecoldwarehouse') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.isoldprocurement')"
                name="isOldProcurement"
              >
                <TaktSelect
                  v-model:value="formState.isOldProcurement"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.isoldprocurement') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.isoldcheck')"
                name="isOldCheck"
              >
                <TaktSelect
                  v-model:value="formState.isOldCheck"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.isoldcheck') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecnewitem')"
                name="ecNewItem"
              >
                <a-input
                  v-model:value="formState.ecNewItem"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecnewitem') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecnewtext')"
                name="ecNewText"
              >
                <a-input
                  v-model:value="formState.ecNewText"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecnewtext') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecnewusage')"
                name="ecNewUsage"
              >
                <a-input-number
                  v-model:value="formState.ecNewUsage"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecnewusage') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecnewposition')"
                name="ecNewPosition"
              >
                <a-input
                  v-model:value="formState.ecNewPosition"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecnewposition') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecnewstock')"
                name="ecNewStock"
              >
                <a-input-number
                  v-model:value="formState.ecNewStock"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ecdetail.ecnewstock') })"
                  :min="0"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecnewwarehouse')"
                name="ecNewWarehouse"
              >
                <TaktSelect
                  v-model:value="formState.ecNewWarehouse"
                  api-url="TaktWarehouses/options"
                  allow-clear
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecnewwarehouse') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.isnewprocurement')"
                name="isNewProcurement"
              >
                <TaktSelect
                  v-model:value="formState.isNewProcurement"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.isnewprocurement') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.isnewcheck')"
                name="isNewCheck"
              >
                <TaktSelect
                  v-model:value="formState.isNewCheck"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.isnewcheck') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ecdetail.ecbomdate')"
                name="ecBomDate"
              >
                <a-date-picker
                  v-model:value="formState.ecBomDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ecdetail.ecbomdate') })"
                  value-format="YYYY-MM-DD"
                  style="width: 100%"
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
 * 设变子表 ecDetail 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/engineering-change/ec-gijutsu/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { EcDetailCreate } from '@/types/logistics/manufacturing/engineering-change/ec-detail'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["ecNo","lineNumber","ecModel","ecBomItem","ecBomItemText","ecBomSubItem","ecBomSubItemText","isEndOfLine","ecOldItem","ecOldText","ecOldUsage","ecOldPosition","ecOldStock","ecOldWarehouse","isOldProcurement","isOldCheck","ecNewItem","ecNewText","ecNewUsage","ecNewPosition","ecNewStock","ecNewWarehouse","isNewProcurement","isNewCheck","ecBomDate"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<EcDetailCreate & { ecDetailId?: string }> | null
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 ecDetailId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.ecDetailId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

      Object.assign(formState, next)
      formRef.value?.clearValidate()
    } else {
      Object.keys(formState).forEach((k) => delete formState[k])
      if (val && typeof val === 'object' && Object.keys(val).length > 0) {
        Object.assign(formState, val)
      }
      formRef.value?.clearValidate()
    }
  },
  { immediate: true }
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  ecNo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecno') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ecdetail.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.ecdetail.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  ecModel: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ecdetail.ecmodel') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 ecId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('ecOldUsage' in payload && payload.ecOldUsage !== undefined && payload.ecOldUsage !== null && payload.ecOldUsage !== '') {
    const raw = payload.ecOldUsage
    payload.ecOldUsage = typeof raw === 'number' ? raw : Number(raw)
  }
  if ('ecNewUsage' in payload && payload.ecNewUsage !== undefined && payload.ecNewUsage !== null && payload.ecNewUsage !== '') {
    const raw = payload.ecNewUsage
    payload.ecNewUsage = typeof raw === 'number' ? raw : Number(raw)
  }
  if ('ecOldStock' in payload && payload.ecOldStock !== undefined && payload.ecOldStock !== null && payload.ecOldStock !== '') {
    const raw = payload.ecOldStock
    payload.ecOldStock = typeof raw === 'number' ? raw : Number(raw)
  }
  if ('ecNewStock' in payload && payload.ecNewStock !== undefined && payload.ecNewStock !== null && payload.ecNewStock !== '') {
    const raw = payload.ecNewStock
    payload.ecNewStock = typeof raw === 'number' ? raw : Number(raw)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.ecId = props.masterId
  return payload
}

/** 重置表单（弹窗未 destroy 时父级 nextTick 也会调用） */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
  }
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
