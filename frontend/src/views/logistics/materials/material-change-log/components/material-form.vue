<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/material-change-log/components -->
<!-- 文件名称：material-form.vue -->
<!-- 功能描述：Takt全局物料实体维护弹窗内嵌表单（上主下从级联保存）。由 generate-vue-master-detail-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form material-form flex flex-col min-h-0"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="material-form-tabs"
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
                :label="t('entity.material.code')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.code') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.materialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.name')"
                name="materialName"
              >
                <a-input
                  v-model:value="formState.materialName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.name') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.specification')"
                name="materialSpecification"
              >
                <a-input
                  v-model:value="formState.materialSpecification"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.specification') })"
                  show-count
                  :maxlength="80"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.material.description')"
                name="materialDescription"
              >
                <a-textarea
                  v-model:value="formState.materialDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.material.description') })"
                  :rows="2"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.industrysector')"
                name="industrySector"
              >
                <TaktSelect
                  v-model:value="formState.industrySector"
                  dict-type="logistics_industry_sector"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.material.industrysector') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.hierarchy')"
                name="materialHierarchy"
              >
                <a-input
                  v-model:value="formState.materialHierarchy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.hierarchy') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.group')"
                name="materialGroup"
              >
                <a-input
                  v-model:value="formState.materialGroup"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.group') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.type')"
                name="materialType"
              >
                <TaktSelect
                  v-model:value="formState.materialType"
                  dict-type="logistics_material_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.material.type') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.material.model')"
                name="materialModel"
              >
                <a-input
                  v-model:value="formState.materialModel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.model') })"
                  show-count
                  :maxlength="100"
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
            <a-col :span="24">
              <a-form-item
                :label="t('entity.material.brand')"
                name="materialBrand"
              >
                <a-input
                  v-model:value="formState.materialBrand"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.brand') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.material.baseunit')"
                name="baseUnit"
              >
                <TaktSelect
                  v-model:value="formState.baseUnit"
                  dict-type="logistics_unit_of_measure_code"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.material.baseunit') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.material.manufacturer')"
                name="manufacturer"
              >
                <a-input
                  v-model:value="formState.manufacturer"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.manufacturer') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.material.manufacturermaterialcode')"
                name="manufacturerMaterialCode"
              >
                <a-input
                  v-model:value="formState.manufacturerMaterialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.manufacturermaterialcode') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                  :disabled="!!formData?.materialId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.material.attributes')"
                name="materialAttributes"
              >
                <a-input
                  v-model:value="formState.materialAttributes"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.material.attributes') })"
                  show-count
                  :maxlength="4000"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.material.isendoflife')"
                name="isEndOfLife"
              >
                <TaktSelect
                  v-model:value="formState.isEndOfLife"
                  dict-type="logistics_material_eol_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.material.isendoflife') })"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.material.status')"
                name="materialStatus"
              >
                <TaktSelect
                  v-model:value="formState.materialStatus"
                  dict-type="sys_normal_disable_status"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.material.status') })"
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
    <!-- 下：子表 changeLogs -->
    <TaktEditableTable
      ref="materialChangeLogTableRef"
      v-model="childMaterialChangeLogRows"
      :columns="materialChangeLogFormColumns"
      :title="t('entity.materialchangelog._self')"
      :add-button-entity="t('entity.materialchangelog._self')"
      id-field="materialChangeLogId"
      :default-row="createDefaultMaterialChangeLogRow"
      :disabled="loading"
      section-border
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt全局物料实体维护表单 · 由 generate-vue-master-detail-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/material-change-log/components
 */
import { reactive, watch, computed, ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { MaterialCreate } from '@/types/logistics/materials/material'
import TaktSelect from '@/components/business/takt-select/index.vue'
import { RiQuestionLine } from '@remixicon/vue'
import { useDictDataStore } from '@/stores/foundation/dict-data'
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
const formFields = ["tenantCode","materialCode","materialName","materialSpecification","materialDescription","industrySector","materialHierarchy","materialGroup","materialType","materialModel","materialBrand","baseUnit","manufacturer","manufacturerMaterialCode","materialAttributes","isEndOfLife","materialStatus","extField","remark"]

import type { TaktEditableTableColumn } from '@/components/business/takt-editable-table/types'

const childMaterialChangeLogRows = ref<Record<string, unknown>[]>([])
const materialChangeLogTableRef = ref<{
  getRows: () => Record<string, unknown>[]
  validate: () => Promise<unknown>
  resetRows: () => void
} | null>(null)

/** 子表 materialChangeLog 可编辑列 */
const materialChangeLogFormColumns = computed<TaktEditableTableColumn[]>(() => [
  {
    key: 'changeFields',
    title: t('entity.materialchangelog.changefields'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.materialchangelog.changefields') }),
  },
  {
    key: 'changeTime',
    title: t('entity.materialchangelog.changetime'),
    editor: 'datePicker',
    valueFormat: 'YYYY-MM-DD HH:mm:ss', showTime: true,
    width: 140,
  },
  {
    key: 'changeBy',
    title: t('entity.materialchangelog.changeby'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.materialchangelog.changeby') }),
  },
  {
    key: 'changeReason',
    title: t('entity.materialchangelog.changereason'),
    editor: 'input',
    width: 140, allowClear: true, placeholder: t('common.page.form.placeholder.optional', { field: t('entity.materialchangelog.changereason') }),
  },
  {
    key: 'extField',
    title: t('common.page.entity.extfield'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.optional', { field: t('common.page.entity.extfield') }),
    width: 140,
  },
  {
    key: 'remark',
    title: t('common.page.entity.remark'),
    editor: 'textarea',
    rows: 2,
    placeholder: t('common.page.form.placeholder.optional', { field: t('common.page.entity.remark') }),
    width: 140,
  },
])

/** 编辑态从 formData 同步各子表行 */
function syncChildRowsFromFormData(val: Partial<MaterialCreate & { materialId?: string }> | null | undefined) {
  childMaterialChangeLogRows.value = ((val as any)?.changeLogs ?? []) as Record<string, unknown>[]
}

function createDefaultMaterialChangeLogRow(): Record<string, unknown> {
  return {
    changeFields: '',
    changeTime: '',
    changeBy: '',
    changeReason: '',
    extField: '',
    remark: '',
  }
}

/** 组装 Create/Update 载荷（主表 + 子表数组） */
function buildSubmitPayload() {
  const masterId = props.formData?.materialId ?? ''
  return {
    ...formState,
    changeLogs: materialChangeLogTableRef.value?.getRows?.() ?? childMaterialChangeLogRows.value.map((rest) => ({
      ...rest,
      tenantCode: tenantStore.tenantCode,
      companyCode: tenantStore.companyCode,
      companyDefaultCulture: userStore.userInfo?.companyDefaultCulture ?? '',
      materialId: masterId,
    })),
  }
}

/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<MaterialCreate & { materialId?: string }> | null
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
/** 表单字段默认值（字典 IsDefault=1，来自 TaktDictDataSeedData） */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  materialType: "ROH",
  materialStatus: 1
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

/** 编辑态灌入 formData；新增态恢复默认值（须含 materialId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.materialId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
    delete (next as any).changeLogs
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
    const isCreate = !props.formData?.materialId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.material.code') }),
      trigger: 'blur'
    }
  ],
  materialName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.material.name') }),
      trigger: 'blur'
    }
  ],
  industrySector: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.industrysector') }),
      trigger: 'change'
    }
  ],
  materialGroup: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.material.group') }),
      trigger: 'blur'
    }
  ],
  materialType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.type') }),
      trigger: 'change'
    }
  ],
  baseUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.baseunit') }),
      trigger: 'change'
    }
  ],
  isEndOfLife: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.material.isendoflife') }),
      trigger: 'change'
    }
  ],
  materialStatus: [{
    validator: async (_rule, value) => {
      if (value === undefined || value === null || value === '') {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.material.status') }))
      }
      const num = typeof value === 'number' ? value : Number(value)
      if (!Number.isFinite(num)) {
        return Promise.reject(t('common.page.form.placeholder.select', { field: t('entity.material.status') }))
      }
      return Promise.resolve()
    },
    trigger: 'change'
  }],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  await materialChangeLogTableRef.value?.validate?.()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  const payload = buildSubmitPayload() as Record<string, unknown>
  if ('materialStatus' in payload) {
    const rawmaterialStatus = payload.materialStatus
    payload.materialStatus = typeof rawmaterialStatus === 'number' ? rawmaterialStatus : Number(rawmaterialStatus)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.materialId)
  childMaterialChangeLogRows.value = []
  materialChangeLogTableRef.value?.resetRows?.()
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
