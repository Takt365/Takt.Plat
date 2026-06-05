<!-- ======================================== -->
<!-- 项目名称：节拍数字工厂 · Takt Digital Factory (TDF) -->
<!-- 命名空间：@/views/logistics/manufacturing/bom/bill-of-material-item/components -->
<!-- 文件名称：bill-of-material-item-form.vue -->
<!-- 功能描述：Takt物料清单明细实体维护弹窗内嵌表单。由 generate-vue-from-api 根据 types/api 自动生成；defineExpose 提供 validate、getValues、resetFields -->
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
      class="bill-of-material-item-form-tabs"
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
                :label="t('entity.billOfMaterialItem.billofmaterialid')"
                name="billOfMaterialId"
              >
                <a-input
                  v-model:value="formState.billOfMaterialId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.billofmaterialid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterialItem.bomcode')"
                name="bomCode"
              >
                <a-input
                  v-model:value="formState.bomCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.bomcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterialItem.linenumber')"
                name="lineNumber"
              >
                <a-input-number
                  v-model:value="formState.lineNumber"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.linenumber') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterialItem.materialid')"
                name="materialId"
              >
                <a-input
                  v-model:value="formState.materialId"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.materialid') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterialItem.materialcode')"
                name="materialCode"
              >
                <a-input
                  v-model:value="formState.materialCode"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.materialcode') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterialItem.usagequantity')"
                name="usageQuantity"
              >
                <a-input-number
                  v-model:value="formState.usageQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.usagequantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterialItem.materialunit')"
                name="materialUnit"
              >
                <a-input
                  v-model:value="formState.materialUnit"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.materialunit') })"
                  size="small"
                  allow-clear
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
                :label="t('entity.billOfMaterialItem.scraprate')"
                name="scrapRate"
              >
                <a-input-number
                  v-model:value="formState.scrapRate"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.scraprate') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterialItem.actualusagequantity')"
                name="actualUsageQuantity"
              >
                <a-input-number
                  v-model:value="formState.actualUsageQuantity"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.actualusagequantity') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterialItem.operationseq')"
                name="operationSeq"
              >
                <a-input-number
                  v-model:value="formState.operationSeq"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.operationseq') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterialItem.workcenter')"
                name="workCenter"
              >
                <a-input
                  v-model:value="formState.workCenter"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.workcenter') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterialItem.position')"
                name="position"
              >
                <a-input
                  v-model:value="formState.position"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.position') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterialItem.substitutegroup')"
                name="substituteGroup"
              >
                <a-input
                  v-model:value="formState.substituteGroup"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.substitutegroup') })"
                  size="small"
                  allow-clear
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterialItem.substitutepriority')"
                name="substitutePriority"
              >
                <a-input-number
                  v-model:value="formState.substitutePriority"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.substitutepriority') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterialItem.isoptional')"
                name="isOptional"
              >
                <a-input-number
                  v-model:value="formState.isOptional"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.isoptional') })"
                  size="small"
                  style="width: 100%"
                />
              </a-form-item>
            </a-col>
            <a-col :span="12">
              <a-form-item
                :label="t('entity.billOfMaterialItem.isphantom')"
                name="isPhantom"
              >
                <a-input-number
                  v-model:value="formState.isPhantom"
                  :placeholder="t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.isphantom') })"
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
 * Takt物料清单明细实体维护表单 · 由 generate-vue-from-api 根据 types/api 生成
 * @module views/logistics/manufacturing/bom/bill-of-material-item/components
 */
import { reactive, watch, computed, ref } from 'vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/es/form'
import type { BillOfMaterialItemCreate } from '@/types/logistics/manufacturing/bom/bill-of-material-item'
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
const formFields = ["tenantCode","companyCode","companyDefaultCulture","billOfMaterialId","bomCode","lineNumber","materialId","materialCode","usageQuantity","materialUnit","scrapRate","actualUsageQuantity","operationSeq","workCenter","position","substituteGroup","substitutePriority","isOptional","isPhantom","extFieldJson","remark"]


interface Props {
  formData?: Partial<BillOfMaterialItemCreate & { billOfMaterialItemId?: string }> | null
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
    const isCreate = !props.formData?.billOfMaterialItemId
    if (isCreate) {
      applyScopeDefaults(formState, true)
    }
  },
)

const rules = computed<Record<string, Rule[]>>(() => ({
  billOfMaterialId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.billofmaterialid') }),
      trigger: 'blur'
    }
  ],
  bomCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.bomcode') }),
      trigger: 'blur'
    }
  ],
  lineNumber: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.billOfMaterialItem.linenumber') }),
      trigger: 'change'
    }
  ],
  materialId: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.materialid') }),
      trigger: 'blur'
    }
  ],
  materialCode: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.materialcode') }),
      trigger: 'blur'
    }
  ],
  usageQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.billOfMaterialItem.usagequantity') }),
      trigger: 'change'
    }
  ],
  materialUnit: [
    {
      required: true,
      message: t('common.page.form.placeholder.required', { field: t('entity.billOfMaterialItem.materialunit') }),
      trigger: 'blur'
    }
  ],
  scrapRate: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.billOfMaterialItem.scraprate') }),
      trigger: 'change'
    }
  ],
  actualUsageQuantity: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.billOfMaterialItem.actualusagequantity') }),
      trigger: 'change'
    }
  ],
  operationSeq: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.billOfMaterialItem.operationseq') }),
      trigger: 'change'
    }
  ],
  substitutePriority: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.billOfMaterialItem.substitutepriority') }),
      trigger: 'change'
    }
  ],
  isOptional: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.billOfMaterialItem.isoptional') }),
      trigger: 'change'
    }
  ],
  isPhantom: [
    {
      required: true,
      message: t('common.page.form.placeholder.select', { field: t('entity.billOfMaterialItem.isphantom') }),
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
