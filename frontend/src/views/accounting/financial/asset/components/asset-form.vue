<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/accounting/financial/asset/components -->
<!-- 文件名称：asset-form.vue -->
<!-- 功能描述：资产实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="asset-form-tabs"
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
                :label="t('entity.asset.code')"
                name="assetCode"
              >
                <a-input
                  v-model:value="formState.assetCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.code') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.name')"
                name="assetName"
              >
                <a-input
                  v-model:value="formState.assetName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.name') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.spec')"
                name="assetSpec"
              >
                <a-input
                  v-model:value="formState.assetSpec"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.asset.spec') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.asset.desc')"
                name="assetDesc"
              >
                <a-textarea
                  v-model:value="formState.assetDesc"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.asset.desc') })"
                  :rows="3"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.category')"
                name="assetCategory"
              >
                <TaktSelect
                  v-model:value="formState.assetCategory"
                  dict-type="accounting_asset_category"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.category') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.type')"
                name="assetType"
              >
                <TaktSelect
                  v-model:value="formState.assetType"
                  dict-type="accounting_asset_type"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.type') })"
                  size="small"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.originalvalue')"
                name="assetOriginalValue"
              >
                <a-input-number
                  v-model:value="formState.assetOriginalValue"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.originalvalue') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.netvalue')"
                name="assetNetValue"
              >
                <a-input-number
                  v-model:value="formState.assetNetValue"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.netvalue') })"
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
                :label="t('entity.asset.accumulateddepreciation')"
                name="accumulatedDepreciation"
              >
                <a-input-number
                  v-model:value="formState.accumulatedDepreciation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.accumulateddepreciation') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.costcenterid')"
                name="costCenterId"
              >
                <a-input
                  v-model:value="formState.costCenterId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.costcenterid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.costcentername')"
                name="costCenterName"
              >
                <a-input
                  v-model:value="formState.costCenterName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.costcentername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.deptid')"
                name="deptId"
              >
                <a-input
                  v-model:value="formState.deptId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.deptid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.deptname')"
                name="deptName"
              >
                <a-input
                  v-model:value="formState.deptName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.deptname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.userid')"
                name="userId"
              >
                <a-input
                  v-model:value="formState.userId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.userid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.username')"
                name="userName"
              >
                <a-input
                  v-model:value="formState.userName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.username') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.location')"
                name="assetLocation"
              >
                <a-input
                  v-model:value="formState.assetLocation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.location') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.purchasedate')"
                name="purchaseDate"
              >
                <a-date-picker
                  v-model:value="formState.purchaseDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.purchasedate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.startdate')"
                name="startDate"
              >
                <a-date-picker
                  v-model:value="formState.startDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.startdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
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
                :label="t('entity.asset.scrapdate')"
                name="scrapDate"
              >
                <a-date-picker
                  v-model:value="formState.scrapDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.scrapdate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.disposaldate')"
                name="disposalDate"
              >
                <a-date-picker
                  v-model:value="formState.disposalDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.asset.disposaldate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.expectedlifemonths')"
                name="expectedLifeMonths"
              >
                <a-input-number
                  v-model:value="formState.expectedLifeMonths"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.expectedlifemonths') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.depreciationmethod')"
                name="depreciationMethod"
              >
                <a-input-number
                  v-model:value="formState.depreciationMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.depreciationmethod') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.monthlydepreciation')"
                name="monthlyDepreciation"
              >
                <a-input-number
                  v-model:value="formState.monthlyDepreciation"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.monthlydepreciation') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.relatedsupplierid')"
                name="relatedSupplierId"
              >
                <a-input
                  v-model:value="formState.relatedSupplierId"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.asset.relatedsupplierid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.relatedsuppliername')"
                name="relatedSupplierName"
              >
                <a-input
                  v-model:value="formState.relatedSupplierName"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.asset.relatedsuppliername') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.relatedplant')"
                name="relatedPlant"
              >
                <a-input
                  v-model:value="formState.relatedPlant"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.relatedplant') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.asset.status')"
                name="assetStatus"
              >
                <a-input-number
                  v-model:value="formState.assetStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.asset.status') })"
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
 * 资产实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/accounting/financial/asset/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { AssetCreate } from '@/types/accounting/financial/asset'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","assetCode","assetName","assetSpec","assetDesc","assetCategory","assetType","assetOriginalValue","assetNetValue","accumulatedDepreciation","costCenterId","costCenterName","deptId","deptName","userId","userName","assetLocation","purchaseDate","startDate","scrapDate","disposalDate","expectedLifeMonths","depreciationMethod","monthlyDepreciation","relatedSupplierId","relatedSupplierName","relatedPlant","assetStatus","extFieldJson","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<AssetCreate & { assetId?: string }> | null
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
    const isCreate = !props.formData?.assetId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  assetCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.asset.code') }),
      trigger: 'blur'
    }
  ],
  assetName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.asset.name') }),
      trigger: 'blur'
    }
  ],
  assetCategory: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.asset.category') }),
      trigger: 'change'
    }
  ],
  assetType: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.asset.type') }),
      trigger: 'change'
    }
  ],
  assetOriginalValue: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.asset.originalvalue') }),
      trigger: 'change'
    }
  ],
  assetNetValue: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.asset.netvalue') }),
      trigger: 'change'
    }
  ],
  accumulatedDepreciation: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.asset.accumulateddepreciation') }),
      trigger: 'change'
    }
  ],
  expectedLifeMonths: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.asset.expectedlifemonths') }),
      trigger: 'change'
    }
  ],
  depreciationMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.asset.depreciationmethod') }),
      trigger: 'change'
    }
  ],
  monthlyDepreciation: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.asset.monthlydepreciation') }),
      trigger: 'change'
    }
  ],
  assetStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.asset.status') }),
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
