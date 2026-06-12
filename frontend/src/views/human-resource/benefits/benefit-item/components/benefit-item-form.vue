<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/benefits/benefit-item/components -->
<!-- 文件名称：benefit-item-form.vue -->
<!-- 功能描述：福利项目维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="benefit-item-form-tabs"
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
                  size="small"
                  readonly
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.companycode')"
                name="companyCode"
              >
                <a-input
                  v-model:value="formState.companyCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companycode') })"
                  size="small"
                  readonly
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.companydefaultculture')"
                name="companyDefaultCulture"
              >
                <a-input
                  v-model:value="formState.companyDefaultCulture"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.companydefaultculture') })"
                  size="small"
                  readonly
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.itemcode')"
                name="itemCode"
              >
                <a-input
                  v-model:value="formState.itemCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.benefititem.itemcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.itemname')"
                name="itemName"
              >
                <a-input
                  v-model:value="formState.itemName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.benefititem.itemname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.category')"
                name="benefitCategory"
              >
                <TaktSelect
                  v-model:value="formState.benefitCategory"
                  dict-type="hr_benefit_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.benefititem.category') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.type')"
                name="benefitType"
              >
                <TaktSelect
                  v-model:value="formState.benefitType"
                  dict-type="hr_benefit_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.benefititem.type') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.paymentcycle')"
                name="paymentCycle"
              >
                <TaktSelect
                  v-model:value="formState.paymentCycle"
                  dict-type="hr_benefit_payment_cycle"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.benefititem.paymentcycle') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.defaultamount')"
                name="defaultAmount"
              >
                <a-input-number
                  v-model:value="formState.defaultAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.benefititem.defaultamount') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.maxamount')"
                name="maxAmount"
              >
                <a-input-number
                  v-model:value="formState.maxAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.benefititem.maxamount') })"
                  size="small"
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
                :label="t('entity.benefititem.employerratio')"
                name="employerRatio"
              >
                <a-input-number
                  v-model:value="formState.employerRatio"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.benefititem.employerratio') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.employeeratio')"
                name="employeeRatio"
              >
                <a-input-number
                  v-model:value="formState.employeeRatio"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.benefititem.employeeratio') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.ismandatory')"
                name="isMandatory"
              >
                <TaktSelect
                  v-model:value="formState.isMandatory"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.benefititem.ismandatory') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.benefititem.sortorder') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.itemstatus')"
                name="itemStatus"
              >
                <TaktSelect
                  v-model:value="formState.itemStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.benefititem.itemstatus') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.benefititem.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.benefititem.relatedplant') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('common.page.entity.extfieldjson')"
                name="extFieldJson"
              >
                <a-input
                  v-model:value="formState.extFieldJson"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('common.page.entity.extfieldjson') })"
                  size="small"
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
                  :rows="2"
                  size="small"
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
 * 福利项目维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/benefits/benefit-item/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { BenefitItemCreate } from '@/types/human-resource/benefits/benefit-item'
import TaktSelect from '@/components/business/takt-select/index.vue'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","itemCode","itemName","benefitCategory","benefitType","paymentCycle","defaultAmount","maxAmount","employerRatio","employeeRatio","isMandatory","sortOrder","itemStatus","relatedPlant","extFieldJson","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<BenefitItemCreate & { benefitItemId?: string }> | null
  /** 父级提交 loading，禁用表单项 */
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false,
})

/** a-form 实例 ref */
const formRef = ref()
/** 表单双向绑定模型 */
const formState = reactive<Record<string, any>>({})

/** 编辑态灌入 formData；新增态 reset */
watch(
  () => props.formData,
  (val) => {
    const next = val ? { ...val } : {}
    Object.keys(formState).forEach((k) => delete formState[k])

    applyScopeDefaults(next)
    Object.assign(formState, next)
  },
  { immediate: true, deep: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.benefitItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  itemCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.benefititem.itemcode') }),
      trigger: 'blur'
    }
  ],
  itemName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.benefititem.itemname') }),
      trigger: 'blur'
    }
  ],
  benefitCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.benefititem.category') }),
      trigger: 'change'
    }
  ],
  benefitType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.benefititem.type') }),
      trigger: 'change'
    }
  ],
  paymentCycle: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.benefititem.paymentcycle') }),
      trigger: 'change'
    }
  ],
  defaultAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.benefititem.defaultamount') }),
      trigger: 'change'
    }
  ],
  maxAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.benefititem.maxamount') }),
      trigger: 'change'
    }
  ],
  employerRatio: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.benefititem.employerratio') }),
      trigger: 'change'
    }
  ],
  employeeRatio: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.benefititem.employeeratio') }),
      trigger: 'change'
    }
  ],
  isMandatory: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.benefititem.ismandatory') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.benefititem.sortorder') }),
      trigger: 'change'
    }
  ],
  itemStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.benefititem.itemstatus') }),
      trigger: 'change'
    }
  ],
}))

/** 校验表单（失败 throw，供父级 handleFormSubmit 捕获） */
async function validate() {
  await formRef.value?.validate()
  return formState
}

/** 映射为 Create/Update DTO */
function getValues(): Record<string, any> {
  return { ...formState }
}

/** 重置表单与子表行 */
function resetFields() {
  formRef.value?.resetFields()
  Object.keys(formState).forEach((k) => delete formState[k])

  activeTab.value = 'tab-0'
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
