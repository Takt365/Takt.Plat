<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/account-title/components -->
<!-- 文件名称：account-title-form.vue -->
<!-- 功能描述：会计科目实体树表维护表单（ParentId + TaktTreeSelect），由 generate-vue-tree-from-api.cjs 自动生成.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="account-title-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/3)'"
        force-render
      >
        <div :class="formContentClass">

          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.accountTitle.parentid')"
                name="parentId"
                :label-col="{ span: 4 }"
                :wrapper-col="{ span: 20 }"
              >
                <TaktTreeSelect
                  v-model:value="formState.parentId"
                  api-url="/api/TaktAccountTitles/tree-options"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accountTitle.parentid') })"
                  allow-clear
                  :field-names="{ label: 'dictLabel', value: 'dictValue' }"
                />
              </a-form-item>
            </a-col>
          </a-row>
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
                
                :disabled="!!formData?.accountTitleId"
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
                
                :disabled="!!formData?.accountTitleId"
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
                :label="t('entity.accountTitle.titlecode')"
                name="titleCode"
              >
                <a-input
                  v-model:value="formState.titleCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titlecode') })"
                  size="small"
                  allow-clear
                
                :disabled="!!formData?.accountTitleId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.titlename')"
                name="titleName"
              >
                <a-input
                  v-model:value="formState.titleName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titlename') })"
                  size="small"
                  allow-clear
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.shortname')"
                name="shortName"
              >
                <a-input
                  v-model:value="formState.shortName"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.accountTitle.shortname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.accountTitle.titledesc')"
                name="titleDesc"
              >
                <a-textarea
                  v-model:value="formState.titleDesc"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.accountTitle.titledesc') })"
                  :rows="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.titletype')"
                name="titleType"
              >
                <a-input-number
                  v-model:value="formState.titleType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titletype') })"
                  size="small"
                  style="width: 100%"
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.balancedirection')"
                name="balanceDirection"
              >
                <a-input-number
                  v-model:value="formState.balanceDirection"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.balancedirection') })"
                  size="small"
                  style="width: 100%"
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.titlelevel')"
                name="titleLevel"
              >
                <a-input-number
                  v-model:value="formState.titleLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titlelevel') })"
                  size="small"
                  style="width: 100%"
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.isauxiliary')"
                name="isAuxiliary"
              >
                <a-input-number
                  v-model:value="formState.isAuxiliary"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.isauxiliary') })"
                  size="small"
                  style="width: 100%"
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.auxiliarytype')"
                name="auxiliaryType"
              >
                <a-input-number
                  v-model:value="formState.auxiliaryType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.auxiliarytype') })"
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
                :label="t('entity.accountTitle.isquantity')"
                name="isQuantity"
              >
                <a-input-number
                  v-model:value="formState.isQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.isquantity') })"
                  size="small"
                  style="width: 100%"
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.iscurrency')"
                name="isCurrency"
              >
                <a-input-number
                  v-model:value="formState.isCurrency"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.iscurrency') })"
                  size="small"
                  style="width: 100%"
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.iscash')"
                name="isCash"
              >
                <a-input-number
                  v-model:value="formState.isCash"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.iscash') })"
                  size="small"
                  style="width: 100%"
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.isbank')"
                name="isBank"
              >
                <a-input-number
                  v-model:value="formState.isBank"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.isbank') })"
                  size="small"
                  style="width: 100%"
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.relatedplant') })"
                  size="small"
                  allow-clear
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.titlestatus')"
                name="titleStatus"
              >
                <TaktSelect
                  v-model:value="formState.titleStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.accountTitle.titlestatus') })"
                  size="small"
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.validfrom')"
                name="validFrom"
              >
                <a-input
                  v-model:value="formState.validFrom"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.validfrom') })"
                  size="small"
                  allow-clear
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.validto')"
                name="validTo"
              >
                <a-input
                  v-model:value="formState.validTo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.accountTitle.validto') })"
                  size="small"
                  allow-clear
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.accountTitle.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.ordernumhint', { field: t('entity.accountTitle.sortorder') })"
                  :min="0"
                  size="small"
                  style="width: 100%"
                
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
 * 会计科目实体维护表单 · 由 generate-vue-tree-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/account-title/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import TaktTreeSelect from '@/components/business/takt-tree-select/index.vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { AccountTitleCreate } from '@/types/accounting/financial/account-title'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","titleCode","titleName","shortName","titleDesc","parentId","titleType","balanceDirection","titleLevel","isAuxiliary","auxiliaryType","isQuantity","isCurrency","isCash","isBank","relatedPlant","titleStatus","validFrom","validTo","sortOrder","extFieldJson","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<AccountTitleCreate & { accountTitleId?: string }> | null
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
    const isCreate = !props.formData?.accountTitleId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  parentId: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.parentid') }),
      trigger: 'change'
    }
  ],
  titleCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titlecode') }),
      trigger: 'blur'
    }
  ],
  titleName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accountTitle.titlename') }),
      trigger: 'blur'
    }
  ],
  parentId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accountTitle.parentid') }),
      trigger: 'blur'
    }
  ],
  titleType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.titletype') }),
      trigger: 'change'
    }
  ],
  balanceDirection: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.balancedirection') }),
      trigger: 'change'
    }
  ],
  titleLevel: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.titlelevel') }),
      trigger: 'change'
    }
  ],
  isAuxiliary: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.isauxiliary') }),
      trigger: 'change'
    }
  ],
  auxiliaryType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.auxiliarytype') }),
      trigger: 'change'
    }
  ],
  isQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.isquantity') }),
      trigger: 'change'
    }
  ],
  isCurrency: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.iscurrency') }),
      trigger: 'change'
    }
  ],
  isCash: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.iscash') }),
      trigger: 'change'
    }
  ],
  isBank: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.isbank') }),
      trigger: 'change'
    }
  ],
  titleStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.titlestatus') }),
      trigger: 'change'
    }
  ],
  validFrom: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accountTitle.validfrom') }),
      trigger: 'blur'
    }
  ],
  validTo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.accountTitle.validto') }),
      trigger: 'blur'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.accountTitle.sortorder') }),
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
