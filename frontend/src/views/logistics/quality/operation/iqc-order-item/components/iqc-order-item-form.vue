<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Plat (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/iqc-order-item/components -->
<!-- 文件名称：iqc-order-item-form.vue -->
<!-- 功能描述：IQC进货检验单明细实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="iqc-order-item-form-tabs"
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
                :label="t('entity.iqcOrderItem.iqcorderid')"
                name="iqcOrderId"
              >
                <a-input
                  v-model:value="formState.iqcOrderId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.iqcorderid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.iqcordercode')"
                name="iqcOrderCode"
              >
                <a-input
                  v-model:value="formState.iqcOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.iqcordercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.materialcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.materialname')"
                name="materialName"
              >
                <a-input
                  v-model:value="formState.materialName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.materialname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.batchno')"
                name="batchNo"
              >
                <a-input
                  v-model:value="formState.batchNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.batchno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.purchasequantity')"
                name="purchaseQuantity"
              >
                <a-input-number
                  v-model:value="formState.purchaseQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.purchasequantity') })"
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
                :label="t('entity.iqcOrderItem.standardcode')"
                name="standardCode"
              >
                <a-input
                  v-model:value="formState.standardCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.standardcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.samplingschemecode')"
                name="samplingSchemeCode"
              >
                <a-input
                  v-model:value="formState.samplingSchemeCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.samplingschemecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.inspectionmethod')"
                name="inspectionMethod"
              >
                <a-input-number
                  v-model:value="formState.inspectionMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.inspectionmethod') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.samplequantity')"
                name="sampleQuantity"
              >
                <a-input-number
                  v-model:value="formState.sampleQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.samplequantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.qualifiedquantity')"
                name="qualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.qualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.qualifiedquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.unqualifiedquantity')"
                name="unqualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.unqualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.unqualifiedquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.inspectionreturnquantity')"
                name="inspectionReturnQuantity"
              >
                <a-input-number
                  v-model:value="formState.inspectionReturnQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.inspectionreturnquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.judgestatus')"
                name="judgeStatus"
              >
                <a-input-number
                  v-model:value="formState.judgeStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.judgestatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.sampleserialno')"
                name="sampleSerialNo"
              >
                <a-input
                  v-model:value="formState.sampleSerialNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.sampleserialno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.iqcOrderItem.inspectiondescription')"
                name="inspectionDescription"
              >
                <a-textarea
                  v-model:value="formState.inspectionDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.iqcOrderItem.inspectiondescription') })"
                  :rows="2"
                  size="small"
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
                :label="t('entity.iqcOrderItem.inspectorby')"
                name="inspectorBy"
              >
                <a-input
                  v-model:value="formState.inspectorBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.inspectorby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.inspectiondate')"
                name="inspectionDate"
              >
                <a-date-picker
                  v-model:value="formState.inspectionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.iqcOrderItem.inspectiondate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.iqcOrderItem.defecthandlings')"
                name="defectHandlings"
              >
                <a-input
                  v-model:value="formState.defectHandlings"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.defecthandlings') })"
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
 * IQC进货检验单明细实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/operation/iqc-order-item/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { IqcOrderItemCreate } from '@/types/logistics/quality/operation/iqc-order-item'
import { useTenantStore } from '@/stores/identity/tenant'
import { useUserStore } from '@/stores/identity/user'

const { t } = useI18n()

const tenantStore = useTenantStore()
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
const formContentClass = computed(() => (formFields.length > 10 ? 'takt-form-content-rows-10' : 'takt-form-content-rows-5'))
const activeTab = ref('tab-0')
const formFields = ["tenantCode","companyCode","companyDefaultCulture","iqcOrderId","iqcOrderCode","lineNumber","materialCode","materialName","batchNo","purchaseQuantity","standardCode","samplingSchemeCode","inspectionMethod","sampleQuantity","qualifiedQuantity","unqualifiedQuantity","inspectionReturnQuantity","judgeStatus","sampleSerialNo","inspectionDescription","inspectorBy","inspectionDate","defectHandlings","extFieldJson","remark"]


interface Props {
  formData?: Partial<IqcOrderItemCreate & { iqcOrderItemId?: string }> | null
  loading?: boolean
}

const props = withDefaults(defineProps<Props>(), {
  formData: () => ({}),
  loading: false
})

const formRef = ref()
const formState = reactive<Record<string, any>>({})

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

watch(
  () => [tenantStore.tenantCode, tenantStore.companyCode, userStore.userInfo?.companyDefaultCulture] as const,
  () => {
    const isCreate = !props.formData?.iqcOrderItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  iqcOrderId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.iqcorderid') }),
      trigger: 'blur'
    }
  ],
  iqcOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.iqcordercode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.iqcOrderItem.linenumber') }),
      trigger: 'change'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.materialcode') }),
      trigger: 'blur'
    }
  ],
  materialName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.materialname') }),
      trigger: 'blur'
    }
  ],
  purchaseQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.iqcOrderItem.purchasequantity') }),
      trigger: 'change'
    }
  ],
  standardCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.standardcode') }),
      trigger: 'blur'
    }
  ],
  samplingSchemeCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.samplingschemecode') }),
      trigger: 'blur'
    }
  ],
  inspectionMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.iqcOrderItem.inspectionmethod') }),
      trigger: 'change'
    }
  ],
  sampleQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.iqcOrderItem.samplequantity') }),
      trigger: 'change'
    }
  ],
  qualifiedQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.iqcOrderItem.qualifiedquantity') }),
      trigger: 'change'
    }
  ],
  unqualifiedQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.iqcOrderItem.unqualifiedquantity') }),
      trigger: 'change'
    }
  ],
  inspectionReturnQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.iqcOrderItem.inspectionreturnquantity') }),
      trigger: 'change'
    }
  ],
  judgeStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.iqcOrderItem.judgestatus') }),
      trigger: 'change'
    }
  ],
  inspectorBy: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.iqcOrderItem.inspectorby') }),
      trigger: 'blur'
    }
  ],
  inspectionDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.iqcOrderItem.inspectiondate') }),
      trigger: 'change'
    }
  ],
}))

async function validate() {
  await formRef.value?.validate()
  return formState
}

function getValues(): Record<string, any> {
  return { ...formState }
}

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
