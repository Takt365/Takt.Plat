<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/controlling/profit-center/components -->
<!-- 文件名称：profit-center-form.vue -->
<!-- 功能描述：利润中心实体树表维护表单（ParentId + TaktTreeSelect），由 generate-vue-tree-from-api.cjs 自动生成.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="profit-center-form-tabs"
    >
      <a-tab-pane
        key="tab-0"
        :tab="t('common.page.form.tabs.basicinfo') + ' (1/2)'"
        force-render
      >
        <div :class="formContentClass">

          <a-row :gutter="24">
            <a-col :span="24">
              <a-form-item
                :label="t('entity.profitCenter.parentid')"
                name="parentId"
                :label-col="{ span: 4 }"
                :wrapper-col="{ span: 20 }"
              >
                <TaktTreeSelect
                  v-model:value="formState.parentId"
                  api-url="/api/TaktProfitCenters/tree-options"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.profitCenter.parentid') })"
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
                
                :disabled="!!formData?.profitCenterId"
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
                
                :disabled="!!formData?.profitCenterId"
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
                :label="t('entity.profitCenter.code')"
                name="profitCenterCode"
              >
                <a-input
                  v-model:value="formState.profitCenterCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.code') })"
                  size="small"
                  allow-clear
                
                :disabled="!!formData?.profitCenterId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.name')"
                name="profitCenterName"
              >
                <a-input
                  v-model:value="formState.profitCenterName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.name') })"
                  size="small"
                  allow-clear
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.shortname')"
                name="shortName"
              >
                <a-input
                  v-model:value="formState.shortName"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.profitCenter.shortname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.profitCenter.profitcenterdesc')"
                name="profitCenterDesc"
              >
                <a-textarea
                  v-model:value="formState.profitCenterDesc"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.profitCenter.profitcenterdesc') })"
                  :rows="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.managerid')"
                name="managerId"
              >
                <a-input
                  v-model:value="formState.managerId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.managerid') })"
                  size="small"
                  allow-clear
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.managername')"
                name="managerName"
              >
                <a-input
                  v-model:value="formState.managerName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.managername') })"
                  size="small"
                  allow-clear
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.deptid')"
                name="deptId"
              >
                <a-input
                  v-model:value="formState.deptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.deptid') })"
                  size="small"
                  allow-clear
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.deptname')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.deptname') })"
                  size="small"
                  allow-clear
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.level')"
                name="profitCenterLevel"
              >
                <a-input-number
                  v-model:value="formState.profitCenterLevel"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.level') })"
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
                :label="t('entity.profitCenter.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.relatedplant') })"
                  size="small"
                  allow-clear
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.status')"
                name="profitCenterStatus"
              >
                <TaktSelect
                  v-model:value="formState.profitCenterStatus"
                  dict-type="sys_normal_disable"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.profitCenter.status') })"
                  size="small"
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.validfrom')"
                name="validFrom"
              >
                <a-input
                  v-model:value="formState.validFrom"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.validfrom') })"
                  size="small"
                  allow-clear
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.validto')"
                name="validTo"
              >
                <a-input
                  v-model:value="formState.validTo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.profitCenter.validto') })"
                  size="small"
                  allow-clear
                
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.profitCenter.sortorder')"
                name="sortOrder"
              >
                <a-input-number
                  v-model:value="formState.sortOrder"
                  :placeholder="t('common.page.form.placeholder.ordernumhint', { field: t('entity.profitCenter.sortorder') })"
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
 * 利润中心实体维护表单 · 由 generate-vue-tree-from-api.cjs 根据 types/api 生成
 * @module views/accounting/controlling/profit-center/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import TaktTreeSelect from '@/components/business/takt-tree-select/index.vue'
import type { Rule } from 'ant-design-vue/es/form'
import type { ProfitCenterCreate } from '@/types/accounting/controlling/profit-center'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","profitCenterCode","profitCenterName","shortName","profitCenterDesc","parentId","managerId","managerName","deptId","deptName","profitCenterLevel","relatedPlant","profitCenterStatus","validFrom","validTo","sortOrder","extFieldJson","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<ProfitCenterCreate & { profitCenterId?: string }> | null
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
    const isCreate = !props.formData?.profitCenterId
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
      message: t('common.page.form.placeholder.select', { field: t('entity.profitCenter.parentid') }),
      trigger: 'change'
    }
  ],
  profitCenterCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.profitCenter.code') }),
      trigger: 'blur'
    }
  ],
  profitCenterName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.profitCenter.name') }),
      trigger: 'blur'
    }
  ],
  parentId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.profitCenter.parentid') }),
      trigger: 'blur'
    }
  ],
  profitCenterLevel: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.profitCenter.level') }),
      trigger: 'change'
    }
  ],
  profitCenterStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.profitCenter.status') }),
      trigger: 'change'
    }
  ],
  validFrom: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.profitCenter.validfrom') }),
      trigger: 'blur'
    }
  ],
  validTo: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.profitCenter.validto') }),
      trigger: 'blur'
    }
  ],
  sortOrder: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.profitCenter.sortorder') }),
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
