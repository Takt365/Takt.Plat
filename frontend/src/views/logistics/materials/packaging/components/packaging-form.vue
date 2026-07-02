<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/materials/packaging/components -->
<!-- 文件名称：packaging-form.vue -->
<!-- 功能描述：Takt物料包装信息实体维护弹窗内嵌表单。由 generate-vue-crud-from-api.cjs 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
<!-- 版权信息：Copyright (c) 2025 Takt  All rights reserved. -->
<!-- 免责声明：此软件使用 MIT License，作者不承担任何使用风险。 -->
<!-- ======================================== -->

<template>
  <a-form
    ref="formRef"
    class="takt-generated-form"
    :model="formState"
    :rules="rules"
    layout="horizontal"
    label-align="right"
  >
    <a-tabs
      v-model:active-key="activeTab"
      class="packaging-form-tabs"
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
                  show-count
                  :maxlength="20"
                  disabled
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
                  show-count
                  :maxlength="20"
                  disabled
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
                  show-count
                  :maxlength="20"
                  disabled
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.plantcode')"
                name="plantCode"
              >
                <a-input
                  v-model:value="formState.plantCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.plantcode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.packagingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.materialcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.packagingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.materialname')"
                name="materialName"
              >
                <a-input
                  v-model:value="formState.materialName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.materialname') })"
                  show-count
                  :maxlength="40"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.hscode')"
                name="hsCode"
              >
                <a-input
                  v-model:value="formState.hsCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.hscode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.packagingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.hsname')"
                name="hsName"
              >
                <a-input
                  v-model:value="formState.hsName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.hsname') })"
                  show-count
                  :maxlength="500"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.additionalcode')"
                name="additionalCode"
              >
                <a-input
                  v-model:value="formState.additionalCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.additionalcode') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                  :disabled="!!formData?.packagingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.origincountryregioncode')"
                name="originCountryRegionCode"
              >
                <a-input
                  v-model:value="formState.originCountryRegionCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.origincountryregioncode') })"
                  show-count
                  :maxlength="2"
                  allow-clear
                  :disabled="!!formData?.packagingId"
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
                :label="t('entity.packaging.origincountryregionname')"
                name="originCountryRegionName"
              >
                <a-input
                  v-model:value="formState.originCountryRegionName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.origincountryregionname') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.destinationcountryregioncode')"
                name="destinationCountryRegionCode"
              >
                <a-input
                  v-model:value="formState.destinationCountryRegionCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.destinationcountryregioncode') })"
                  show-count
                  :maxlength="2"
                  allow-clear
                  :disabled="!!formData?.packagingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.destinationcountryregionname')"
                name="destinationCountryRegionName"
              >
                <a-input
                  v-model:value="formState.destinationCountryRegionName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.destinationcountryregionname') })"
                  show-count
                  :maxlength="100"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.regulatoryconditioncode')"
                name="regulatoryConditionCode"
              >
                <a-input
                  v-model:value="formState.regulatoryConditionCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.regulatoryconditioncode') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                  :disabled="!!formData?.packagingId"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.tariffratetype')"
                name="tariffRateType"
              >
                <a-input
                  v-model:value="formState.tariffRateType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.tariffratetype') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.grossweight')"
                name="grossWeight"
              >
                <a-input-number
                  v-model:value="formState.grossWeight"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.grossweight') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.netweight')"
                name="netWeight"
              >
                <a-input-number
                  v-model:value="formState.netWeight"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.netweight') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.weightunit')"
                name="weightUnit"
              >
                <a-input
                  v-model:value="formState.weightUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.weightunit') })"
                  show-count
                  :maxlength="10"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.businessvolume')"
                name="businessVolume"
              >
                <a-input-number
                  v-model:value="formState.businessVolume"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.businessvolume') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.packaging.volumeunit')"
                name="volumeUnit"
              >
                <a-input
                  v-model:value="formState.volumeUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.volumeunit') })"
                  show-count
                  :maxlength="10"
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
                :label="t('entity.packaging.sizedimension')"
                name="sizeDimension"
              >
                <a-input
                  v-model:value="formState.sizeDimension"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.sizedimension') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.packaging.type')"
                name="packagingType"
              >
                <a-input
                  v-model:value="formState.packagingType"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.type') })"
                  show-count
                  :maxlength="50"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.packaging.packingunit')"
                name="packingUnit"
              >
                <a-input
                  v-model:value="formState.packingUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.packingunit') })"
                  show-count
                  :maxlength="20"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.packaging.quantityperpacking')"
                name="quantityPerPacking"
              >
                <a-input-number
                  v-model:value="formState.quantityPerPacking"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.quantityperpacking') })"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.packaging.spec')"
                name="packagingSpec"
              >
                <a-input
                  v-model:value="formState.packagingSpec"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.packaging.spec') })"
                  show-count
                  :maxlength="200"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.packaging.description')"
                name="packagingDescription"
              >
                <a-textarea
                  v-model:value="formState.packagingDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.packaging.description') })"
                  :rows="2"
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
  </a-form>
</template>

<script setup lang="ts">
/**
 * Takt物料包装信息实体维护表单 · 由 generate-vue-crud-from-api.cjs 根据 types/api 生成
 * @module views/logistics/materials/packaging/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { PackagingCreate } from '@/types/logistics/materials/packaging'
import { RiQuestionLine } from '@remixicon/vue'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","plantCode","materialCode","materialName","hsCode","hsName","additionalCode","originCountryRegionCode","originCountryRegionName","destinationCountryRegionCode","destinationCountryRegionName","regulatoryConditionCode","tariffRateType","grossWeight","netWeight","weightUnit","businessVolume","volumeUnit","sizeDimension","packagingType","packingUnit","quantityPerPacking","packagingSpec","packagingDescription","extField","remark"]


/** 父级传入的编辑 DTO；新增时为 undefined 或空对象 */
interface Props {
  formData?: Partial<PackagingCreate & { packagingId?: string }> | null
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
/** 表单字段默认值（无字典默认项） */
function applyFormDefaults(target: Record<string, unknown>) {
  void target
}


/** 编辑态灌入 formData；新增态恢复默认值（须含 packagingId 才视为编辑） */
watch(
  () => props.formData,
  (val) => {
    if (val?.packagingId) {
      const next = { ...val } as Record<string, unknown>
      Object.keys(formState).forEach((k) => delete formState[k])

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
  { immediate: true }
)

/** 公司/租户切换时，新增态表单同步隔离字段 */
watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.packagingId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

/** 表单校验规则（与 FluentValidation 必填对齐） */
const rules = computed<Record<string, Rule[]>>(() => ({
  plantCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.packaging.plantcode') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.packaging.materialcode') }),
      trigger: 'blur'
    }
  ],
  materialName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.packaging.materialname') }),
      trigger: 'blur'
    }
  ],
  weightUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.packaging.weightunit') }),
      trigger: 'blur'
    }
  ],
  volumeUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.packaging.volumeunit') }),
      trigger: 'blur'
    }
  ],
  packagingType: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.packaging.type') }),
      trigger: 'blur'
    }
  ],
  packingUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.packaging.packingunit') }),
      trigger: 'blur'
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
  const payload = { ...formState }
  if ('grossWeight' in payload) {
    const rawgrossWeight = payload.grossWeight
    payload.grossWeight = typeof rawgrossWeight === 'number' ? rawgrossWeight : Number(rawgrossWeight)
  }
  if ('netWeight' in payload) {
    const rawnetWeight = payload.netWeight
    payload.netWeight = typeof rawnetWeight === 'number' ? rawnetWeight : Number(rawnetWeight)
  }
  if ('businessVolume' in payload) {
    const rawbusinessVolume = payload.businessVolume
    payload.businessVolume = typeof rawbusinessVolume === 'number' ? rawbusinessVolume : Number(rawbusinessVolume)
  }
  if ('quantityPerPacking' in payload) {
    const rawquantityPerPacking = payload.quantityPerPacking
    payload.quantityPerPacking = typeof rawquantityPerPacking === 'number' ? rawquantityPerPacking : Number(rawquantityPerPacking)
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
  applyScopeDefaults(formState as Record<string, unknown>, !props.formData?.packagingId)

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
