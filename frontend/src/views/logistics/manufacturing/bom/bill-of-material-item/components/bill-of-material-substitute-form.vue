<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material-item/components -->
<!-- 文件名称：bill-of-material-substitute-form.vue -->
<!-- 功能描述：Takt物料清单明细实体子表 billOfMaterialSubstitute 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form bill-of-material-substitute-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="bill-of-material-substitute-form-tabs"
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
                :label="t('entity.billofmaterialsubstitute.billofmaterialid')"
                name="billOfMaterialId"
              >
                <a-input
                  v-model:value="formState.billOfMaterialId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.billofmaterialid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialsubstitute.bomcode')"
                name="bomCode"
              >
                <a-input
                  v-model:value="formState.bomCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.bomcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.billOfMaterialSubstituteId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialsubstitute.primarymaterialcode')"
                name="primaryMaterialCode"
              >
                <a-input
                  v-model:value="formState.primaryMaterialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.primarymaterialcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.billOfMaterialSubstituteId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialsubstitute.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialsubstitute.substitutematerialid')"
                name="substituteMaterialId"
              >
                <TaktSelect
                  v-model:value="formState.substituteMaterialId"
                  :options="filteredMaterialPlantOptions"
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.billofmaterialsubstitute.substitutematerialid') })"
                  :disabled="loading || !resolvedPlantCode"
                  allow-clear
                  @change="handleSubstituteMaterialChangeForm"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialsubstitute.substitutematerialcode')"
                name="substituteMaterialCode"
              >
                <a-input
                  v-model:value="formState.substituteMaterialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.substitutematerialcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.billOfMaterialSubstituteId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialsubstitute.substitutegroup')"
                name="substituteGroup"
              >
                <a-input
                  v-model:value="formState.substituteGroup"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.substitutegroup') })"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billofmaterialsubstitute.substitutepriority')"
                name="substitutePriority"
              >
                <a-input-number
                  v-model:value="formState.substitutePriority"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.substitutepriority') })"
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
 * Takt物料清单明细实体子表 billOfMaterialSubstitute 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/manufacturing/bom/bill-of-material-item/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { BillOfMaterialSubstituteCreate } from '@/types/logistics/manufacturing/bom/bill-of-material-substitute'
import type { TaktSelectOption } from '@/types/common'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { getMaterialPlantOptions, getMaterialPlantById } from '@/api/logistics/materials/material-plant'
import { getBillOfMaterialItemById } from '@/api/logistics/manufacturing/bom/bill-of-material-item'
import { getBillOfMaterialById } from '@/api/logistics/manufacturing/bom/bill-of-material'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 工厂物料下拉全量选项 */
const materialPlantOptions = ref<TaktSelectOption[]>([])
/** 由 BOM 头解析的工厂代码 */
const resolvedPlantCode = ref('')
/** 按工厂过滤的工厂物料选项 */
const filteredMaterialPlantOptions = computed(() => {
  if (!resolvedPlantCode.value) {
    return materialPlantOptions.value
  }
  return materialPlantOptions.value.filter((item) => String(item.extValue ?? '') === String(resolvedPlantCode.value))
})

/** 加载工厂物料选项 */
async function loadMaterialPlantOptions() {
  materialPlantOptions.value = await getMaterialPlantOptions()
}

/** 根据明细/BOM 解析工厂代码 */
async function resolvePlantCode() {
  const itemId = props.masterId || formState.billOfMaterialItemId
  if (!itemId) {
    resolvedPlantCode.value = ''
    return
  }
  try {
    const item = await getBillOfMaterialItemById(String(itemId))
    if (item?.billOfMaterialId) {
      const bom = await getBillOfMaterialById(item.billOfMaterialId)
      resolvedPlantCode.value = bom?.plantCode ?? ''
    }
  } catch {
    resolvedPlantCode.value = ''
  }
}

/** 替代物料选择变更（独立表单） */
async function handleSubstituteMaterialChangeForm(materialId: string | number | undefined) {
  if (materialId == null || materialId === '') {
    formState.substituteMaterialCode = ''
    return
  }
  const option = materialPlantOptions.value.find((item) => String(item.dictValue) === String(materialId))
  if (option?.extLabel) {
    formState.substituteMaterialCode = option.extLabel
  }
  try {
    const mp = await getMaterialPlantById(String(materialId))
    if (mp?.materialCode) {
      formState.substituteMaterialCode = mp.materialCode
    }
  } catch {
    /* 选项冗余字段已回填 */
  }
}

onMounted(() => {
  void loadMaterialPlantOptions()
  void resolvePlantCode()
})
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["billOfMaterialId","bomCode","primaryMaterialCode","lineNumber","substituteMaterialId","substituteMaterialCode","substituteGroup","substitutePriority"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<BillOfMaterialSubstituteCreate & { billOfMaterialSubstituteId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 billOfMaterialSubstituteId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.billOfMaterialSubstituteId) {
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

watch(
  () => props.masterId,
  () => {
    void resolvePlantCode()
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  billOfMaterialId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.billofmaterialid') }),
      trigger: 'blur'
    }
  ],
  bomCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.bomcode') }),
      trigger: 'blur'
    }
  ],
  primaryMaterialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.primarymaterialcode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialsubstitute.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialsubstitute.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  substituteMaterialId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.substitutematerialid') }),
      trigger: 'blur'
    }
  ],
  substituteMaterialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billofmaterialsubstitute.substitutematerialcode') }),
      trigger: 'blur'
    }
  ],
  substitutePriority: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialsubstitute.substitutepriority') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.billofmaterialsubstitute.substitutepriority') }))
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

/** 映射为 Create/Update DTO（含主表外键 billOfMaterialItemId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('substitutePriority' in payload) {
    const rawsubstitutePriority = payload.substitutePriority
    payload.substitutePriority = typeof rawsubstitutePriority === 'number' ? rawsubstitutePriority : Number(rawsubstitutePriority)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.billOfMaterialItemId = props.masterId
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
