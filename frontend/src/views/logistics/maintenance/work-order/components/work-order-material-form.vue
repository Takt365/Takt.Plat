<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/maintenance/work-order/components -->
<!-- 文件名称：work-order-material-form.vue -->
<!-- 功能描述：维护工单实体子表 maintenanceWorkOrderMaterial 独立 CRUD 弹窗表单；defineExpose validate/getValues/resetFields。由 generate-vue-master-detail-from-api.cjs 生成，风格与主表 *-form 一致 -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form work-order-material-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="work-order-material-form-tabs"
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
                :label="t('entity.maintenanceworkordermaterial.workordercode')"
                name="workOrderCode"
              >
                <a-input
                  v-model:value="formState.workOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.workordercode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkordermaterial.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.linenumber') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkordermaterial.materialid')"
                name="materialId"
              >
                <a-input
                  v-model:value="formState.materialId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.materialid') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkordermaterial.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.materialcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.maintenanceWorkOrderMaterialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkordermaterial.materialname')"
                name="materialName"
              >
                <a-input
                  v-model:value="formState.materialName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.materialname') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkordermaterial.requiredquantity')"
                name="requiredQuantity"
              >
                <a-input-number
                  v-model:value="formState.requiredQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.requiredquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkordermaterial.issuedquantity')"
                name="issuedQuantity"
              >
                <a-input-number
                  v-model:value="formState.issuedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.issuedquantity') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.maintenanceworkordermaterial.materialunit')"
                name="materialUnit"
              >
                <a-input
                  v-model:value="formState.materialUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.materialunit') })"
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
 * 维护工单实体子表 maintenanceWorkOrderMaterial 维护表单 · 由 generate-vue-master-detail-from-api.cjs 生成
 * @module views/logistics/maintenance/work-order/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { MaintenanceWorkOrderMaterialCreate } from '@/types/logistics/maintenance/work-order-material'

/** i18n 翻译函数 */
const { t } = useI18n()
/** 表单内容区高度 class（字段多时 tab-10 行） */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')
/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ["workOrderCode","lineNumber","materialId","materialCode","materialName","requiredQuantity","issuedQuantity","materialUnit"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaintenanceWorkOrderMaterialCreate & { maintenanceWorkOrderMaterialId?: string }> | null
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


/** 编辑态灌入 formData；新增态恢复默认值（须含 maintenanceWorkOrderMaterialId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.maintenanceWorkOrderMaterialId) {
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
  workOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.workordercode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkordermaterial.linenumber') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkordermaterial.linenumber') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.materialid') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.materialcode') }),
      trigger: 'blur'
    }
  ],
  materialName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.materialname') }),
      trigger: 'blur'
    }
  ],
  requiredQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkordermaterial.requiredquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkordermaterial.requiredquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  issuedQuantity: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkordermaterial.issuedquantity') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.maintenanceworkordermaterial.issuedquantity') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
  materialUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.maintenanceworkordermaterial.materialunit') }),
      trigger: 'blur'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（含主表外键 maintenanceWorkOrderId） */
function getValues(): Record<string, any> {
  const payload = { ...formState }
  if ('lineNumber' in payload) {
    const rawlineNumber = payload.lineNumber
    payload.lineNumber = typeof rawlineNumber === 'number' ? rawlineNumber : Number(rawlineNumber)
  }
  if ('requiredQuantity' in payload) {
    const rawrequiredQuantity = payload.requiredQuantity
    payload.requiredQuantity = typeof rawrequiredQuantity === 'number' ? rawrequiredQuantity : Number(rawrequiredQuantity)
  }
  if ('issuedQuantity' in payload) {
    const rawissuedQuantity = payload.issuedQuantity
    payload.issuedQuantity = typeof rawissuedQuantity === 'number' ? rawissuedQuantity : Number(rawissuedQuantity)
  }
  if ('sortOrder' in payload) delete payload.sortOrder
  payload.maintenanceWorkOrderId = props.masterId
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
