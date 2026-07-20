<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/material-cost/components -->
<!-- 文件名称：material-cost-form.vue -->
<!-- 功能描述：BOM 物料成本汇总表弹窗表单（明细独立维护于右侧 panel，按业务键关联）；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form material-cost-form flex flex-col min-h-0 overflow-visible"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="material-cost-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
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
                :label="pi.label('tenantCode')"
                name="tenantCode"
              >
                <a-input
                  v-model:value="formState.tenantCode"
                  :placeholder="pi.ph('tenantCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyCode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="pi.ph('companyCode')"
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="pi.label('companyDefaultCulture')"
                name="companyDefaultCulture"
              >
                <a-input
                  v-model:value="formState.companyDefaultCulture"
                  :placeholder="pi.ph('companyDefaultCulture')"
                  show-count
                  :maxlength="20"
                  disabled
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
                    <span>{{ pi.label('extField') }}</span>
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
                :label="pi.label('remark')"
                name="remark"
              >
                <a-textarea
                  v-model:value="formState.remark"
                  :placeholder="pi.ph('remark')"
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
    <a-alert
      type="info"
      show-icon
      class="mt-3"
      :message="t('logistics.manufacturing.bom.material-cost.page.modalmasterhint')"
    />
  </a-form>
</template>

<script setup lang="ts">
/**
 * BOM 物料成本主表抬头表单（明细在右侧 panel）
 * @module views/logistics/manufacturing/bom/material-cost/components
 */
import type { Rule } from 'ant-design-vue/es/form'
import { useI18n } from 'vue-i18n'
import type { BomMaterialCostCreate } from '@/types/logistics/manufacturing/bom/material-cost'
import { useDictDataStore } from '@/stores/foundation/dict-data'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'
import { useBomMaterialCostI18n } from '../composables/use-material-cost-i18n'

/** 实体字段 i18n */
const pi = useBomMaterialCostI18n()
/** i18n 翻译函数 */
const { t } = useI18n()
/** Pinia：租户/公司上下文 */
const tenantStore = useTenantStore()
/** Pinia：用户上下文 */
const userStore = useUserStore()
/** Pinia：字典缓存 */
const dictDataStore = useDictDataStore()

/** CreateDto 字段名列表（与 formState 键对齐） */
const formFields = ['tenantCode','companyCode','companyDefaultCulture','plantCode','modelCode','modelMonthlyAverageCost','productCode','productDescription','productMonthlyCost','currencyCode','costingPeriod','costingDate','extField','remark']

/**
 * 上下文隔离字段：租户 / 公司 / 公司默认语言
 * @param target 表单数据
 * @param force 为 true 时强制覆盖
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

/** 表单内容区高度 class */
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
/** 当前激活的 Tab key */
const activeTab = ref('tab-0')

/** 父级传入的编辑 DTO */
interface Props {
  formData?: Partial<BomMaterialCostCreate & { bomMaterialCostId?: string }> | null
  /** 父级提交 loading */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: null,
  loading: false,
})

/** a-form 实例 */
const formRef = ref()
/** 表单模型 */
const formState = reactive<Record<string, any>>({})
/** 字段默认值 */
const FORM_FIELD_DEFAULTS: Record<string, string | number> = {
  currencyCode: 'CNY',
  modelMonthlyAverageCost: 0,
  productMonthlyCost: 0,
}

/**
 * 写入表单默认值
 * @param target 目标对象
 */
function applyFormDefaults(target: Record<string, unknown>) {
  Object.assign(target, FORM_FIELD_DEFAULTS)
}

onMounted(() => {
  void dictDataStore.loadAllDictDataAsync()
})

watch(
  () => props.formData,
  (val) => {
    if (val?.bomMaterialCostId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])
      delete (next as { items?: unknown }).items
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
  { immediate: true },
)

watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    if (!props.formData?.bomMaterialCostId) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 校验规则 */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [{ required: true, message: pi.ph('plantCode'), trigger: 'change' }],
  modelCode: [{ required: true, message: pi.ph('modelCode'), trigger: 'blur' }],
  productCode: [{ required: true, message: pi.ph('productCode'), trigger: 'blur' }],
  productDescription: [{ required: true, message: pi.ph('productDescription'), trigger: 'blur' }],
  currencyCode: [{ required: true, message: pi.ph('currencyCode'), trigger: 'change' }],
  costingDate: [{ required: true, message: pi.ph('costingDate'), trigger: 'change' }],
}))

/** 校验表单 */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO（不含 items，明细由右侧 panel 维护） */
function getValues(): Record<string, any> {
  const payload = { ...formState } as Record<string, unknown>
  for (const key of ['modelMonthlyAverageCost', 'productMonthlyCost'] as const) {
    if (key in payload) {
      const raw = payload[key]
      payload[key] = typeof raw === 'number' ? raw : Number(raw)
    }
  }
  delete payload.items
  delete payload.sortOrder
  return payload
}

/** 重置表单 */
function resetFields() {
  Object.keys(formState).forEach((k) => delete formState[k])
  if (props.formData && typeof props.formData === 'object') {
    Object.assign(formState, props.formData)
    delete (formState as { items?: unknown }).items
  }
  applyFormDefaults(formState)
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.bomMaterialCostId)
  activeTab.value = 'tab-0'
  formRef.value?.clearValidate()
}

defineExpose({ validate, getValues, resetFields })
</script>

<style scoped lang="css">
:deep(.ant-tabs-content-holder) {
  min-height: 40vh;
}
:deep(.ant-tabs-tabpane) {
  min-height: 40vh;
}
</style>
