<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/quality/operation/ipqc-order-item/components -->
<!-- 文件名称：ipqc-order-item-form.vue -->
<!-- 功能描述：IPQC制程检验单明细实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="ipqc-order-item-form-tabs"
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
                :label="t('entity.ipqcOrderItem.ipqcorderid')"
                name="ipqcOrderId"
              >
                <a-input
                  v-model:value="formState.ipqcOrderId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.ipqcorderid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.ipqcordercode')"
                name="ipqcOrderCode"
              >
                <a-input
                  v-model:value="formState.ipqcOrderCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.ipqcordercode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.materialcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.materialname')"
                name="materialName"
              >
                <a-input
                  v-model:value="formState.materialName"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.materialname') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.batchno')"
                name="batchNo"
              >
                <a-input
                  v-model:value="formState.batchNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.batchno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.productionquantity')"
                name="productionQuantity"
              >
                <a-input-number
                  v-model:value="formState.productionQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.productionquantity') })"
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
                :label="t('entity.ipqcOrderItem.standardcode')"
                name="standardCode"
              >
                <a-input
                  v-model:value="formState.standardCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.standardcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.samplingschemecode')"
                name="samplingSchemeCode"
              >
                <a-input
                  v-model:value="formState.samplingSchemeCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.samplingschemecode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.inspectionmethod')"
                name="inspectionMethod"
              >
                <a-input-number
                  v-model:value="formState.inspectionMethod"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.inspectionmethod') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.samplequantity')"
                name="sampleQuantity"
              >
                <a-input-number
                  v-model:value="formState.sampleQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.samplequantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.qualifiedquantity')"
                name="qualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.qualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.qualifiedquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.unqualifiedquantity')"
                name="unqualifiedQuantity"
              >
                <a-input-number
                  v-model:value="formState.unqualifiedQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.unqualifiedquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.inspectionreturnquantity')"
                name="inspectionReturnQuantity"
              >
                <a-input-number
                  v-model:value="formState.inspectionReturnQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.inspectionreturnquantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.judgestatus')"
                name="judgeStatus"
              >
                <a-input-number
                  v-model:value="formState.judgeStatus"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.judgestatus') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.sampleserialno')"
                name="sampleSerialNo"
              >
                <a-input
                  v-model:value="formState.sampleSerialNo"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.sampleserialno') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="24">
              <a-form-item
                :label="t('entity.ipqcOrderItem.inspectiondescription')"
                name="inspectionDescription"
              >
                <a-textarea
                  v-model:value="formState.inspectionDescription"
                  :placeholder="t('common.page.form.placeholder.optional', { field: t('entity.ipqcOrderItem.inspectiondescription') })"
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
                :label="t('entity.ipqcOrderItem.inspectorby')"
                name="inspectorBy"
              >
                <a-input
                  v-model:value="formState.inspectorBy"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.inspectorby') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.inspectiondate')"
                name="inspectionDate"
              >
                <a-date-picker
                  v-model:value="formState.inspectionDate"
                  :placeholder="t('common.page.form.placeholder.select', { field: t('entity.ipqcOrderItem.inspectiondate') })"
                  value-format="YYYY-MM-DD"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.ipqcOrderItem.defecthandlings')"
                name="defectHandlings"
              >
                <a-input
                  v-model:value="formState.defectHandlings"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.defecthandlings') })"
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
 * IPQC制程检验单明细实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/quality/operation/ipqc-order-item/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { IpqcOrderItemCreate } from '@/types/logistics/quality/operation/ipqc-order-item'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","ipqcOrderId","ipqcOrderCode","lineNumber","materialCode","materialName","batchNo","productionQuantity","standardCode","samplingSchemeCode","inspectionMethod","sampleQuantity","qualifiedQuantity","unqualifiedQuantity","inspectionReturnQuantity","judgeStatus","sampleSerialNo","inspectionDescription","inspectorBy","inspectionDate","defectHandlings","extFieldJson","remark"]


interface Props {
  formData?: Partial<IpqcOrderItemCreate & { ipqcOrderItemId?: string }> | null
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
    const isCreate = !props.formData?.ipqcOrderItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  ipqcOrderId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.ipqcorderid') }),
      trigger: 'blur'
    }
  ],
  ipqcOrderCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.ipqcordercode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrderItem.linenumber') }),
      trigger: 'change'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.materialcode') }),
      trigger: 'blur'
    }
  ],
  materialName: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.materialname') }),
      trigger: 'blur'
    }
  ],
  productionQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrderItem.productionquantity') }),
      trigger: 'change'
    }
  ],
  standardCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.standardcode') }),
      trigger: 'blur'
    }
  ],
  samplingSchemeCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.samplingschemecode') }),
      trigger: 'blur'
    }
  ],
  inspectionMethod: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrderItem.inspectionmethod') }),
      trigger: 'change'
    }
  ],
  sampleQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrderItem.samplequantity') }),
      trigger: 'change'
    }
  ],
  qualifiedQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrderItem.qualifiedquantity') }),
      trigger: 'change'
    }
  ],
  unqualifiedQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrderItem.unqualifiedquantity') }),
      trigger: 'change'
    }
  ],
  inspectionReturnQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrderItem.inspectionreturnquantity') }),
      trigger: 'change'
    }
  ],
  judgeStatus: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrderItem.judgestatus') }),
      trigger: 'change'
    }
  ],
  inspectorBy: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.ipqcOrderItem.inspectorby') }),
      trigger: 'blur'
    }
  ],
  inspectionDate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.ipqcOrderItem.inspectiondate') }),
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
