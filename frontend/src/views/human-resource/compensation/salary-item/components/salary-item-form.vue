<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/human-resource/compensation/salary-item/components -->
<!-- 文件名称：salary-item-form.vue -->
<!-- 功能描述：薪资项目维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="salary-item-form-tabs"
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
                :label="t('entity.salaryitem.itemcode')"
                name="itemCode"
              >
                <a-input
                  v-model:value="formState.itemCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.itemcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.itemname')"
                name="itemName"
              >
                <a-input
                  v-model:value="formState.itemName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.itemname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.shortname')"
                name="shortName"
              >
                <a-input
                  v-model:value="formState.shortName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.shortname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.itemtype')"
                name="itemType"
              >
                <TaktSelect
                  v-model:value="formState.itemType"
                  dict-type="hr_salary_item_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.itemtype') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.calcmethod')"
                name="calcMethod"
              >
                <TaktSelect
                  v-model:value="formState.calcMethod"
                  dict-type="hr_salary_calc_method"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.calcmethod') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.salaryformulaid')"
                name="salaryFormulaId"
              >
                <a-input
                  v-model:value="formState.salaryFormulaId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.salaryformulaid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.defaultamount')"
                name="defaultAmount"
              >
                <a-input-number
                  v-model:value="formState.defaultAmount"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.defaultamount') })"
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
        :tab="t('common.page.form.tabs.basicinfo') + ' (2/3)'"
        force-render
      >
        <div :class="formContentClass">
          <a-row :gutter="24">
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.defaultrate')"
                name="defaultRate"
              >
                <a-input-number
                  v-model:value="formState.defaultRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.defaultrate') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.strikeprice')"
                name="strikePrice"
              >
                <a-input-number
                  v-model:value="formState.strikePrice"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.strikeprice') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.vestingyears')"
                name="vestingYears"
              >
                <a-input-number
                  v-model:value="formState.vestingYears"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.vestingyears') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.isdeduction')"
                name="isDeduction"
              >
                <TaktSelect
                  v-model:value="formState.isDeduction"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.isdeduction') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.istaxable')"
                name="isTaxable"
              >
                <TaktSelect
                  v-model:value="formState.isTaxable"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.istaxable') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.includesocialsecuritybase')"
                name="includeSocialSecurityBase"
              >
                <TaktSelect
                  v-model:value="formState.includeSocialSecurityBase"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.includesocialsecuritybase') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.includehousingfundbase')"
                name="includeHousingFundBase"
              >
                <TaktSelect
                  v-model:value="formState.includeHousingFundBase"
                  dict-type="sys_yes_no"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.includehousingfundbase') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.sortorder') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.itemstatus')"
                name="itemStatus"
              >
                <TaktSelect
                  v-model:value="formState.itemStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.salaryitem.itemstatus') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.salaryitem.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.salaryitem.relatedplant') })"
                  size="small"
                  allow-clear
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
 * 薪资项目维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/human-resource/compensation/salary-item/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { SalaryItemCreate } from '@/types/human-resource/compensation/salary-item'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","itemCode","itemName","shortName","itemType","calcMethod","salaryFormulaId","defaultAmount","defaultRate","strikePrice","vestingYears","isDeduction","isTaxable","includeSocialSecurityBase","includeHousingFundBase","sortOrder","itemStatus","relatedPlant","extFieldJson","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<SalaryItemCreate & { salaryItemId?: string }> | null
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
    const isCreate = !props.formData?.salaryItemId
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
      message: t('common.page.form.placeholder.required', { field: t('entity.salaryitem.itemcode') }),
      trigger: 'blur'
    }
  ],
  itemName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.salaryitem.itemname') }),
      trigger: 'blur'
    }
  ],
  itemType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salaryitem.itemtype') }),
      trigger: 'change'
    }
  ],
  calcMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salaryitem.calcmethod') }),
      trigger: 'change'
    }
  ],
  defaultAmount: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salaryitem.defaultamount') }),
      trigger: 'change'
    }
  ],
  defaultRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salaryitem.defaultrate') }),
      trigger: 'change'
    }
  ],
  strikePrice: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salaryitem.strikeprice') }),
      trigger: 'change'
    }
  ],
  vestingYears: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salaryitem.vestingyears') }),
      trigger: 'change'
    }
  ],
  isDeduction: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salaryitem.isdeduction') }),
      trigger: 'change'
    }
  ],
  isTaxable: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salaryitem.istaxable') }),
      trigger: 'change'
    }
  ],
  includeSocialSecurityBase: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salaryitem.includesocialsecuritybase') }),
      trigger: 'change'
    }
  ],
  includeHousingFundBase: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salaryitem.includehousingfundbase') }),
      trigger: 'change'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salaryitem.sortorder') }),
      trigger: 'change'
    }
  ],
  itemStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.salaryitem.itemstatus') }),
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
